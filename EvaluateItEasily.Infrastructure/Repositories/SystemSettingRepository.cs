namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class SystemSettingRepository : GenericRepository<SystemSetting>, ISystemSettingRepository
    {
        public SystemSettingRepository(AppDbContext context) : base(context) { }

        public async Task<SystemSetting?> GetByKeyAsync(string key, CancellationToken ct = default) =>
            await _context.SystemSettings
                .FirstOrDefaultAsync(s => s.Key == key, ct);
    }
}
