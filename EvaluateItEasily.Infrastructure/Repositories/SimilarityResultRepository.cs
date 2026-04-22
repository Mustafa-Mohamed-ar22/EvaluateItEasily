

namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class SimilarityResultRepository : GenericRepository<SimilarityResult>, ISimilarityResultRepository
    {
        public SimilarityResultRepository(AppDbContext context) : base(context) { }

        public async Task DeleteByEvaluationIdAsync(int evaluationId, CancellationToken ct = default)
        {
            var results = await _context.SimilarityResults
                .Where(sr => sr.EvaluationId == evaluationId)
                .ToListAsync(ct);

            if (results.Any())
                _context.SimilarityResults.RemoveRange(results);
        }
    }
}