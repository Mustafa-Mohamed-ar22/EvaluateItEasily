namespace EvaluateItEasily.Core.Entities
{
    public class SupervisorAssignment : AuditableEntity
    {
        public int Id { get; set; }

        public int ProposalId { get; set; }
        public Proposal Proposal { get; set; } = default!;

        public string SupervisorId { get; set; } = string.Empty;   
        public ApplicationUser Supervisor { get; set; } = default!;

        // tect
        public string TechnicalAssistantId { get; set; } = string.Empty;   
        public ApplicationUser TechnicalAssistant { get; set; } = default!;
        public string AssignedById { get; set; } = string.Empty;   
        public ApplicationUser AssignedByUser { get; set; } = default!;

        public string? WorkloadNote { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
    }
}
