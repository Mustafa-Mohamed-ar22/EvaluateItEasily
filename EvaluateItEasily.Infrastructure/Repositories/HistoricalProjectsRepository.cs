namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class HistoricalProjectsRepository : GenericRepository<HistoricalProject>, IHistoricalProjectsRepository
    {
        public HistoricalProjectsRepository(AppDbContext context) : base(context)
        {
        }

        async Task IHistoricalProjectsRepository.BulkInsertAsync(IEnumerable<HistoricalProject> projects, CancellationToken ct)
        => await _context.AddRangeAsync(projects, ct);

        async Task<IEnumerable<HistoricalProject>> IHistoricalProjectsRepository.GetAllWithDetailsAsync(CancellationToken ct)
        => await _context.HistoricalProjects.OrderByDescending(x=>x.ArchivedAt).ToListAsync(ct);

        async Task<HistoricalProject?> IHistoricalProjectsRepository.GetByProjectIdAsync(int projectId, CancellationToken ct)
        => await _context.HistoricalProjects.FirstOrDefaultAsync(x=>x.Id==projectId, ct);
    }
}
