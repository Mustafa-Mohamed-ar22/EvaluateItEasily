using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface ISystemSettingRepository : IGenericRepository<SystemSetting>
    {
        Task<SystemSetting?> GetByKeyAsync(string key, CancellationToken ct = default);
    }
}
