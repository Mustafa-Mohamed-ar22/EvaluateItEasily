using EvaluateItEasily.Core.DTO_s.Account;
using EvaluateItEasily.Core.DTO_s.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class UserService(
    UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService,IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,IEmailSender emailSender,ICacheService cacheService) : IUserService          // ← inject cache
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailService = emailSender;
        private readonly ICacheService _cacheService = cacheService;

        private const string AllUsersCacheKey = "users:all";
        private static string UserCacheKey(string id) => $"users:{id}";
        private static string UsersByRoleCacheKey(string role) => $"users:role:{role.ToLower()}";


        public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync(string? role,CancellationToken ct = default)
        {
            var cacheKey = string.IsNullOrEmpty(role) ? AllUsersCacheKey : UsersByRoleCacheKey(role);

            var cached = await _cacheService.GetAsync<IEnumerable<UserResponse>>(cacheKey, ct);
            if (cached is not null)
                return Result.Success(cached);

            IList<ApplicationUser> users;

            if (!string.IsNullOrEmpty(role))
                users = await _userManager.GetUsersInRoleAsync(role);
            else
                users = await _userManager.Users.OrderBy(u => u.FullName).ToListAsync(ct);

            var response = new List<UserResponse>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(MapToResponse(user, roles.FirstOrDefault() ?? string.Empty));
            }

            await _cacheService.SetAsync(cacheKey, response, ct);

            return Result.Success<IEnumerable<UserResponse>>(response);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(string id,CancellationToken ct = default)
        {
            var cached = await _cacheService.GetAsync<UserResponse>(UserCacheKey(id), ct);
            if (cached is not null)
                return Result.Success(cached);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.NotFound);

            var roles = await _userManager.GetRolesAsync(user);
            var response = MapToResponse(user, roles.FirstOrDefault() ?? string.Empty);

            await _cacheService.SetAsync(UserCacheKey(id), response, ct);

            return Result.Success(response);
        }

        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
        {
            var existing = await _userManager.FindByEmailAsync(request.Email);
            if (existing is not null)
                return Result.Failure<UserResponse>(UserErrors.EmailAlreadyExists);

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result.Failure<UserResponse>(new Error("User.CreationFailed", errors, StatusCodes.Status500InternalServerError));
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            try
            {
                await SendEmail(user, code, request.Password);
            }
            catch (FormatException)
            {
                return Result.Failure<UserResponse>(AuthErrors.FaliedToSendEmail);
            }

            var response = MapToResponse(user, request.Role);

            await _cacheService.RemoveAsync(AllUsersCacheKey, ct);
            await _cacheService.RemoveAsync(UsersByRoleCacheKey(request.Role), ct);
            // ✅ Removed redundant SetAsync for individual user

            return Result.Success(response);
        }

        public async Task<Result<UserResponse>> UpdateAsync(string id, UpdateUserRequest request, CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.NotFound);

            if (!user.Email!.Equals(request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _userManager.FindByEmailAsync(request.Email);
                if (existing is not null)
                    return Result.Failure<UserResponse>(UserErrors.EmailAlreadyExists);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? string.Empty;

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.UpdatedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            var response = MapToResponse(user, userRole);

            await _cacheService.RemoveAsync(AllUsersCacheKey, ct);
            await _cacheService.RemoveAsync(UsersByRoleCacheKey(userRole), ct); 
            await _cacheService.SetAsync(UserCacheKey(id), response, ct);

            return Result.Success(response);
        }
        public async Task<Result> ToggleActiveAsync(string id,CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            var currentUserId = _currentUserService.GetUserId();
            if (user.Id == currentUserId)
                return Result.Failure(UserErrors.CannotDeactivateSelf);

            user.IsActive = !user.IsActive;
            user.UpdatedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? string.Empty;

            await _cacheService.RemoveAsync(AllUsersCacheKey, ct);
            await _cacheService.RemoveAsync(UsersByRoleCacheKey(userRole), ct);
            await _cacheService.RemoveAsync(UserCacheKey(id), ct);

            return Result.Success();
        }

        public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId!);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        private async Task SendEmail(ApplicationUser user, string code, string password)
        {
            var httpRequest = _httpContextAccessor.HttpContext?.Request;
            var origin = $"{httpRequest?.Scheme}://{httpRequest?.Host}";

            var emailBody = EmailBodyBuilder.GenerateEmailBody(
                _webHostEnvironment.ContentRootPath,
                "TemplateWelcomeEmail",
                new Dictionary<string, string>
                {
                    { "{{name}}",     user.FullName },
                    { "{{email}}",    user.Email! },
                    { "{{password}}", password }
                });

            await _emailService.SendEmailAsync(user.Email!,"✅ EvaluateItEasily : Welcome Email",emailBody);
        }
        private static UserResponse MapToResponse(ApplicationUser user, string role) => new(
            Id: user.Id,
            FullName: user.FullName,
            Email: user.Email!,
            Role: role,
            IsActive: user.IsActive,
            CreatedOn: user.CreatedOn
        );
    }
}
