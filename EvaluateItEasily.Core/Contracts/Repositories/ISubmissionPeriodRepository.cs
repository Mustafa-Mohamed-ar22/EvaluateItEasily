using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface ISubmissionPeriodRepository : IGenericRepository<SubmissionPeriod>
    {
        Task<SubmissionPeriod?> GetCurrentOpenAsync(CancellationToken ct = default);
        Task<SubmissionPeriod?> GetActiveAsync(CancellationToken ct = default);
        Task<IEnumerable<SubmissionPeriod>> GetAllAsync(CancellationToken ct = default);
        Task<bool> HasOverlapAsync(DateTime start, DateTime end, int? excludeId = null, CancellationToken ct = default);
    }
}
