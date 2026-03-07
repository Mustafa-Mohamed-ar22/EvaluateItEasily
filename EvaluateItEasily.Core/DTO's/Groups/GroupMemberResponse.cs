namespace EvaluateItEasily.Core.DTO_s.Groups
{
    public record GroupMemberResponse(
    string StudentId,
    string FullName,
    string Email,
    bool IsLeader,
    DateTime JoinedAt
);
}
