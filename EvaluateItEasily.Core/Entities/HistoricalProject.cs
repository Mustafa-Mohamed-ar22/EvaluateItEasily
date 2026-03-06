using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Core.Entities
{
    public class HistoricalProject : AuditableEntity
    {
        public int Id { get; set; }

        public int? ProposalId { get; set; }
        public Proposal? Proposal { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Abstract { get; set; } = string.Empty;
        public string GroupName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public DateTime ArchivedAt { get; set; }

        public ICollection<SimilarityResult> SimilarityResults { get; set; } = [];
    }
}
