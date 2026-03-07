namespace EvaluateItEasily.Core.DTO_s.Groups
{
    public record GroupResponse(
    int Id,
    string Name,
    string LeaderId,
    string LeaderName,
    int MembersCount,
    DateTime CreatedOn,
    IEnumerable<GroupMemberResponse> Members
    );
}
