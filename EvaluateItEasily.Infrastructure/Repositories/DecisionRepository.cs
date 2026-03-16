namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class DecisionRepository : GenericRepository<Decision>, IDecisionRepository
    {
        public DecisionRepository(AppDbContext context) : base(context) { }

        public async Task<Decision?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default) =>
            await _context.Decisions
                .Include(d => d.Proposal)
                .Include(d => d.DecidedByUser)
                .FirstOrDefaultAsync(d => d.ProposalId == proposalId, ct);
        public async Task<IEnumerable<Decision>> GetByDecisionTypeAsync(
            DecisionType decisionType,
            CancellationToken ct = default) =>
            await _context.Decisions
                .Include(d => d.Proposal)
                .Include(d => d.DecidedByUser)
                .Where(d => d.DecisionType == decisionType)
                .OrderByDescending(d => d.DecidedAt)
                .ToListAsync(ct);
    }
}
