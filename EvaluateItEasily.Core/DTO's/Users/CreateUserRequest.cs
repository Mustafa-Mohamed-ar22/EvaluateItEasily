
namespace EvaluateItEasily.Core.DTO_s.Users
{
    public record CreateUserRequest(
    string FullName,
    string Email,
    string Password,
    string Role        
);
}
