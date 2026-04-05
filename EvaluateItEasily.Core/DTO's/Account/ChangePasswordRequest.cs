namespace EvaluateItEasily.Core.DTO_s.Account
{
    public record ChangePasswordRequest
    (
        string CurrentPassword,
        string NewPassword
        );
}
