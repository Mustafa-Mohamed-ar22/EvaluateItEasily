
using EvaluateItEasily.Core.DTO_s.Account;
using EvaluateItEasily.Core.DTO_s.Users;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class UserService(UserManager<ApplicationUser> userManager,ICurrentUserService currentUserService) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var currentUserId = _currentUserService.GetUserId();
            var user = await _userManager.FindByIdAsync(currentUserId!);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code,error.Description,StatusCodes.Status400BadRequest));
        }

        public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync(string? role,CancellationToken ct = default)
        {
            IList<ApplicationUser> users;

            if (!string.IsNullOrEmpty(role))
            {
                users = await _userManager.GetUsersInRoleAsync(role);
            }
            else
            {
                users = await _userManager.Users
                    .OrderBy(u => u.FullName)
                    .ToListAsync(ct);
            }
            var response = new List<UserResponse>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                response.Add(MapToResponse(user, roles.FirstOrDefault() ?? string.Empty));
            }
            return Result.Success<IEnumerable<UserResponse>>(response);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(string id,CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.NotFound);

            var roles = await _userManager.GetRolesAsync(user);
            return Result.Success(MapToResponse(user, roles.FirstOrDefault() ?? string.Empty));
        }

        public async Task<Result<UserResponse>> CreateAsync(CreateUserRequest request,CancellationToken ct = default)
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
                EmailConfirmed = true            // eeslsdkjvcsl;kdjmlk
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result.Failure<UserResponse>(new Error("User.CreationFailed", errors, StatusCodes.Status500InternalServerError));
            }

            await _userManager.AddToRoleAsync(user, request.Role);

            return Result.Success(MapToResponse(user, request.Role));
        }

        public async Task<Result<UserResponse>> UpdateAsync(string id,UpdateUserRequest request,CancellationToken ct = default)
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

            user.FullName = request.FullName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.UpdatedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            return Result.Success(MapToResponse(user, roles.FirstOrDefault() ?? string.Empty));
        }

        public async Task<Result> ToggleActiveAsync(string id,CancellationToken ct = default)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure(UserErrors.NotFound);

            // prevent admin from deactivating themselves
            var currentUserId = _currentUserService.GetUserId();
            if (user.Id == currentUserId)
                return Result.Failure(UserErrors.CannotDeactivateSelf);

            user.IsActive = !user.IsActive;
            user.UpdatedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return Result.Success();
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
