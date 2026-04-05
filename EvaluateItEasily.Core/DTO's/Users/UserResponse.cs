
namespace EvaluateItEasily.Core.DTO_s.Users
{
    public record UserResponse
    (
        string Id,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    DateTime CreatedOn
        );
}
