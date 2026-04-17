namespace EvaluateItEasily.Core.DTO_s.Groups
{
    public record GroupResponse(
    int Id,
    string Name,
    string LeaderId,
    string LeaderName,
    int MembersCount,
    DateTime CreatedOn,
    int? ProposalId,
    string? ProposalStatus,
    string? SupervisorName,            
    string? TechnicalAssistantName,    
    IEnumerable<GroupMemberResponse> Members
    );
}
