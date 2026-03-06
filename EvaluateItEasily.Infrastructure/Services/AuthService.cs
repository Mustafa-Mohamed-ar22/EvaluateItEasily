using EvaluateItEasily.Core.Auth;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Identity;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        private readonly JwtProvider _jwtProvider;

        public AuthService(UserManager<ApplicationUser> userManager, JwtProvider jwtProvider, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
            _roleManager = roleManager;
        }

        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default!)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials);
            if(!user.IsActive)
                return Result.Failure<AuthResponse>(AuthErrors.InactiveUser);
            var isValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!isValid)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidCredentials);
            return await BuildAuthResponseAsync(user);
        }


        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser is not null)
                return Result.Failure<AuthResponse>(AuthErrors.EmailAlreadyExists);

            var user = new ApplicationUser
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Result.Failure<AuthResponse>(AuthErrors.CreationFailed);
            }
            await _userManager.AddToRoleAsync(user, "Student");

            return await BuildAuthResponseAsync(user);
        }
        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            // Find user who owns this token
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));

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

            await _userManager.UpdateAsync(user);

            return await BuildAuthResponseAsync(user, newRefreshToken.Token);
        }

        public async Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshTokens.Any(x => x.Token == refreshToken));

            if (user is null)
                return Result.Failure(AuthErrors.InvalidToken);

            var token = user.RefreshTokens.Single(x => x.Token == refreshToken);

            if (!token.IsActive)
                return Result.Failure(AuthErrors.InvalidToken);

            token.RevokedIn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Result.Success();
        }
        private async Task<Result<AuthResponse>> BuildAuthResponseAsync(ApplicationUser user,string? existingRefreshToken = null)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jwtProvider.GenerateAccessTaoken(user, roles);

            string refreshTokenValue;

            if (existingRefreshToken is not null)
            {
                refreshTokenValue = existingRefreshToken;
            }
            else
            {
                var newRefreshToken = JwtProvider.GenerateRefreshToken();
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
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
