
namespace EvaluateItEasily.Core.DTO_s.SupervisorAssignments
{
    public record CreateSupervisorAssignmentRequest
    (
    int ProposalId,
    string SupervisorId,
    string TechnicalAssistantId,
    string? WorkloadNote
    );
}
