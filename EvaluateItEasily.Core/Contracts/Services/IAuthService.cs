using EvaluateItEasily.Core.Auth;
using EvaluateItEasily.Core.DTO_s.Auth;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
        Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
        Task<Result> SendResetPasswordCodeAsync(string email);
        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
