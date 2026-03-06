using EvaluateItEasily.Core.Enums;

namespace EvaluateItEasily.Core.Entities
{
    public class Proposal : AuditableEntity
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; } = default!;

        public string Title { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string ProposalFileUrl { get; set; } = string.Empty;
        public ProposalStatus Status { get; set; } = ProposalStatus.Pending;
        public DateTime SubmittedAt { get; set; }

        public Evaluation? Evaluation { get; set; }
        public Decision? Decision { get; set; }
        public SupervisorAssignment? SupervisorAssignment { get; set; }
        public HistoricalProject? HistoricalProject { get; set; }
    }
}
