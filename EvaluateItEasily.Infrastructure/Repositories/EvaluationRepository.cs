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
        public async Task DeleteByProposalIdAsync(int proposalId, CancellationToken ct = default)
        {
            var evaluation = await _context.Evaluations
                .FirstOrDefaultAsync(e => e.ProposalId == proposalId, ct);

            if (evaluation is not null)
            {
                var similarityResults = await _context.SimilarityResults
                    .Where(sr => sr.EvaluationId == evaluation.Id)
                    .ToListAsync(ct);

                if (similarityResults.Any())
                    _context.SimilarityResults.RemoveRange(similarityResults);

                _context.Evaluations.Remove(evaluation);
            }
        }
    }
}