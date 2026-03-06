using EvaluateItEasily.Core.Enums;
namespace EvaluateItEasily.Core.Entities
{
    public class Evaluation : AuditableEntity
    {
        public int Id { get; set; }

        public int ProposalId { get; set; }
        public Proposal Proposal { get; set; } = default!;

        public string EvaluatedById { get; set; } = string.Empty; 
        public ApplicationUser EvaluatedByUser { get; set; } = default!;

        public AIEvaluationStatus AIStatus { get; set; } = AIEvaluationStatus.Pending;
        public float MaxSimilarityScore { get; set; }
        public DateTime EvaluatedAt { get; set; }

        public ICollection<SimilarityResult> SimilarityResults { get; set; } = [];
    }
}
