namespace EvaluateItEasily.Core.DTO_s
{
    public record SupervisorAssignmentResponse
    (
        int Id,
    int ProposalId,
    string ProposalTitle,
    string GroupName,
    string SupervisorId,
    string SupervisorName,
    string SupervisorEmail,
    string AssignedByName,
    string? WorkloadNote,
    DateTime AssignedAt,

        string TechnicalAssistantId, 
        string TechnicalAssistantName,   
        string TechnicalAssistantEmail  
        );
}
