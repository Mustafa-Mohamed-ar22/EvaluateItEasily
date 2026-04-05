namespace EvaluateItEasily.Core.DTO_s.Auth
{
    public record ResetPasswordRequest
    (
        string Email,
        string Code,
        string NewPassword
        );
}

