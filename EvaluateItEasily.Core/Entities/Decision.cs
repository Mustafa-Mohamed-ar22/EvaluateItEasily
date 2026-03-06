using EvaluateItEasily.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Core.Entities
{
    public class Decision : AuditableEntity
    {
        public int Id { get; set; }

        public int ProposalId { get; set; }
        public Proposal Proposal { get; set; } = default!;

        public string DecidedById { get; set; } = string.Empty;   
        public ApplicationUser DecidedByUser { get; set; } = default!;

        public DecisionType DecisionType { get; set; }
        public string FeedbackComment { get; set; } = string.Empty;
        public DateTime DecidedAt { get; set; }
    }
}
