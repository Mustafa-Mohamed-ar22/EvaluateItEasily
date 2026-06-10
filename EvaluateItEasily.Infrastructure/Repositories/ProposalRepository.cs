
namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class ProposalRepository : GenericRepository<Proposal>, IProposalRepository
    {
        public ProposalRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<IEnumerable<Proposal>> GetAllWithDetailsAsync(string? status = null
            ,CancellationToken ct = default)
        {
            var query = _context.Proposals
                .Include(p => p.Group)
                    .ThenInclude(g => g.Leader)
                .Include(p => p.Group)
                    .ThenInclude(g => g.Members)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) &&Enum.TryParse<ProposalStatus>(status, ignoreCase: true,
                out var parsedStatus))
                query = query.Where(p => p.Status == parsedStatus);

            return await query
                .OrderByDescending(p => p.SubmittedAt)
                .ToListAsync(ct);
        }

        public async Task<Proposal?> GetByGroupIdAsync(int groupId, CancellationToken ct = default)
        => await _context.Proposals
            .Include(p => p.Group)
                .ThenInclude(g => g.Leader)
            .Include(p => p.Group)
                .ThenInclude(g => g.Members)
            .FirstOrDefaultAsync(p => p.GroupId == groupId, ct);

        public async Task<Proposal?> GetWithDetailsAsync(int id, CancellationToken ct = default)
        =>
            await _context.Proposals.Include(x => x.Group).ThenInclude(x => x.Leader)
            .Include(x => x.Group).ThenInclude(x => x.Members)
            .FirstOrDefaultAsync(x => x.Id == id, ct);


        public async Task<IEnumerable<Proposal>> GetAcceptedNotArchivedAsync(CancellationToken ct = default)
        => await _context.Proposals.Include(x=>x.Group)
            .Where(x => x.Status == ProposalStatus.Accepted && x.HistoricalProject == null)
            .ToListAsync(ct);

    }
}