
using EvaluateItEasily.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, JwtProvider jwtProvider,
        RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor,IEmailSender emailService,IWebHostEnvironment webHostEnvironment,
        IOptions<DomainCORS> options) : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailService = emailService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly DomainCORS _domainOptions = options.Value;

        public async Task<Result<AuthResponse>> LoginAsync(Core.Auth.LoginRequest request, CancellationToken ct = default!)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials);
            if(!user.IsActive)
                return Result.Failure<AuthResponse>(AuthErrors.InactiveUser);
            var result = await signInManager.PasswordSignInAsync(user, request.Password,false,false);
            if(result.Succeeded)
                return await BuildAuthResponseAsync(user);
            
            
            return Result.Failure<AuthResponse>(result.IsNotAllowed ? AuthErrors.EmailNotConfirmed : AuthErrors.InvalidCredentials);
        }


        public async Task<Result> RegisterAsync(Core.Auth.RegisterRequest request, CancellationToken ct = default)
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Result.Failure<AuthResponse>(AuthErrors.EmailAlreadyExists);

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.First();
                return Result.Failure<AuthResponse>(new Error(errors.Code,errors.Description,StatusCodes.Status400BadRequest));
            }
            await userManager.AddToRoleAsync(user, UserRole.Student.ToString());

            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            try
            {
                await SendEmail(user, code);
            }
            catch (FormatException)
            {
                return Result.Failure(AuthErrors.FaliedToSendEmail);
            }

            return await BuildAuthResponseAsync(user);
        }
        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            if (await userManager.FindByIdAsync(request.UserId) is not { } user)
                return Result.Failure(AuthErrors.InvalideCode);
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.AlreadyConfirmed);

            var code = request.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(AuthErrors.InvalideCode);
            }

            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }
        public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
        {
            if (await userManager.FindByEmailAsync(request.Email) is not { } user)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.AlreadyConfirmed);
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            try
            {
                await SendEmail(user, code);
            }
            catch (FormatException)
            {
                return Result.Failure(AuthErrors.FaliedToSendEmail);
            }

            return Result.Success();
        }
        public async Task<Result> SendResetPasswordCodeAsync(string email)
        {
            if(await userManager.FindByEmailAsync(email) is not { } user)
                return Result.Success();
            if (!user.EmailConfirmed)
                return Result.Failure(AuthErrors.EmailNotConfirmed);
            var code = await userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            try
            {
                await SendResetPassword(user, code);
            }
            catch (FormatException)
            {
                return Result.Failure(AuthErrors.FaliedToSendEmail);
            }

            return Result.Success();
        }
        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user =await userManager.FindByEmailAsync(request.Email);
            if (user is null || !user.EmailConfirmed)
                return Result.Failure(AuthErrors.InvalideCode);//mis
            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await userManager.ResetPasswordAsync(user,code,request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(userManager.ErrorDescriber.InvalidToken());
            }
            if(result.Succeeded)
                return Result.Success();
            var error = result.Errors.First();
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));

        }
        private async Task SendEmail(ApplicationUser user, string code)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var origin = $"{request?.Scheme}://{request?.Host}";
            var emailBody = EmailBodyBuilder.GenerateEmailBody(_webHostEnvironment.ContentRootPath,"TemplateSendEmail", new Dictionary<string, string>
            {
                {"{{name}}",user.FullName},
                {"{{action_url}}",$"{_domainOptions.Domain1}/auth/emailConfirmation?userId={user.Id}&code={code}"}
            });

            await _emailService.SendEmailAsync(user.Email!, "✅ EvaluateItEasily : Verification Email", emailBody);
        }
        private async Task SendResetPassword(ApplicationUser user, string code)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var origin = $"{request?.Scheme}://{request?.Host}";
            var emailBody = EmailBodyBuilder.GenerateEmailBody(_webHostEnvironment.ContentRootPath, "ForgetPasswordTemplate", new Dictionary<string, string>
            {
                {"{{name}}",user.FullName},
                {"{{action_url}}",$"{_domainOptions.Domain1}/auth/forgetPassword?email={user.Email}&code={code}"}
            });
            
            await _emailService.SendEmailAsync(user.Email!, "✅ EvaluateItEasily : Forget Password", emailBody);
        }
        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            // Find user who owns this token
            var user = userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidToken);

            // Find the token
            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!token.IsActive)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidToken);

            // Revoke old token
            token.RevokedIn = DateTime.UtcNow;

            // Generate new refresh token
            var newRefreshToken = JwtProvider.GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            await userManager.UpdateAsync(user);

            return await BuildAuthResponseAsync(user, newRefreshToken.Token);
        }

        public async Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            var user = userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(x => x.Token == refreshToken));

            if (user is null)
                return Result.Failure(AuthErrors.InvalidToken);

            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!token.IsActive)
                return Result.Failure(AuthErrors.InvalidToken);

            token.RevokedIn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            return Result.Success();
        }
        private async Task<Result<AuthResponse>> BuildAuthResponseAsync(ApplicationUser user,string? existingRefreshToken = null)
        {
            var roles = await userManager.GetRolesAsync(user);
            var accessToken = jwtProvider.GenerateAccessTaoken(user, roles);

            string refreshTokenValue;

            if (existingRefreshToken is not null)
            {
                refreshTokenValue = existingRefreshToken;
            }
            else
            {
                var newRefreshToken = JwtProvider.GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await userManager.UpdateAsync(user);
                refreshTokenValue = newRefreshToken.Token;
            }

            return Result.Success(new AuthResponse(
                UserId: user.Id,
                Email: user.Email!,
                FullName: user.FullName,
                Role: roles.FirstOrDefault() ?? string.Empty,
                AccessToken: accessToken,
                RefreshToken: refreshTokenValue
            ));
        }
    }
}
