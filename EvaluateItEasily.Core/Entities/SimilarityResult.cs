using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Core.Entities
{
    public class SimilarityResult
    {
        public int Id { get; set; }

        public int EvaluationId { get; set; }
        public Evaluation Evaluation { get; set; } = default!;

        public int HistoricalProjectId { get; set; }
        public HistoricalProject HistoricalProject { get; set; } = default!;

        public float SimilarityScore { get; set; }
        public int Rank { get; set; }

    }
}
