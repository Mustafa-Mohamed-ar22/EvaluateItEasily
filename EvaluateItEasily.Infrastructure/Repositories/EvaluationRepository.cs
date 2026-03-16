

namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class EvaluationRepository : GenericRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(AppDbContext context) : base(context) { }

        public async Task<Evaluation?> GetWithResultsAsync(int proposalId, CancellationToken ct = default) =>
            await _context.Evaluations
                .Include(e => e.EvaluatedByUser)
                .Include(e => e.Proposal)
                .Include(e => e.SimilarityResults)
                    .ThenInclude(sr => sr.HistoricalProject)
                .FirstOrDefaultAsync(e => e.ProposalId == proposalId, ct);
        public async Task<IEnumerable<Evaluation>> GetAllWithResultsAsync(CancellationToken ct = default) =>
            await _context.Evaluations
                .Include(e => e.EvaluatedByUser)
                .Include(e => e.Proposal)
                .Include(e => e.SimilarityResults)
                    .ThenInclude(sr => sr.HistoricalProject).ToListAsync(ct);
    }
}