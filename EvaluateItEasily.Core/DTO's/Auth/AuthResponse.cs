namespace EvaluateItEasily.Core.Auth
{
    public record AuthResponse(
    string UserId,
    string Email,
    string FullName,
    string Role,
    string AccessToken,
    string RefreshToken
    );
}
