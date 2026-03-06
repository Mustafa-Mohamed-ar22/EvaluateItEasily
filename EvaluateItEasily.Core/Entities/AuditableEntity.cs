using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Core.Entities
{
    public class AuditableEntity
    {
        public string CreatedById { get; set; } = string.Empty;
        public ApplicationUser CreatedBy { get; set; } = default!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public string? UpdatedById { get; set; }
        public ApplicationUser? UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedOn { get; set; }
    }
}
