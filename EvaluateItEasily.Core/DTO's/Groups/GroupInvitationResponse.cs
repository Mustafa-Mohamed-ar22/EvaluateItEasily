
namespace EvaluateItEasily.Core.DTO_s.Groups
{
    public record GroupInvitationResponse(
    int Id,
    int GroupId,
    string GroupName,
    string LeaderName,
    string InvitedStudentId,
    string InvitedStudentName,
    string InvitedStudentEmail,
    string Status,
    DateTime CreatedOn,
    DateTime? RespondedAt
);
}
