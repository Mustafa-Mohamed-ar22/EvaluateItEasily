using EvaluateItEasily.Core.Auth;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<Result> RevokeTokenAsync(string refreshToken, CancellationToken ct = default);
    }
}
