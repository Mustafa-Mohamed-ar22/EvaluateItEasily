namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class SubmissionPeriodRepository : GenericRepository<SubmissionPeriod>, ISubmissionPeriodRepository
    {
        public SubmissionPeriodRepository(AppDbContext context) : base(context) { }

        // Period that is open RIGHT NOW
        public async Task<SubmissionPeriod?> GetCurrentOpenAsync(CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;
            return await _context.SubmissionPeriods
                .Include(sp => sp.CreatedBy)
                .FirstOrDefaultAsync(sp => sp.IsActive
                    && sp.StartDate <= now
                    && sp.EndDate >= now, ct);
        }

        // Any period marked active (regardless of dates)
        public async Task<SubmissionPeriod?> GetActiveAsync(CancellationToken ct = default) =>
            await _context.SubmissionPeriods
                .Include(sp => sp.CreatedBy)
                .FirstOrDefaultAsync(sp => sp.IsActive, ct);

        public async Task<IEnumerable<SubmissionPeriod>> GetAllAsync(CancellationToken ct = default) =>
            await _context.SubmissionPeriods
                .Include(sp => sp.CreatedBy)
                .OrderByDescending(sp => sp.StartDate)
                .ToListAsync(ct);

        public async Task<bool> HasOverlapAsync(DateTime start,DateTime end,int? excludeId = null,CancellationToken ct = default) =>
            await _context.SubmissionPeriods
                .AnyAsync(sp => sp.IsActive
                    && (excludeId == null || sp.Id != excludeId)
                    && sp.StartDate < end
                    && sp.EndDate > start, ct);
    }
}
