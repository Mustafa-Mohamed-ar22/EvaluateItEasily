using EvaluateItEasily.Core.Entities;
namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IHistoricalProjectsRepository : IGenericRepository<HistoricalProject>
    {
        Task<HistoricalProject?> GetByProjectIdAsync(int projectId, CancellationToken ct = default);
        Task<IEnumerable<HistoricalProject>> GetAllWithDetailsAsync(CancellationToken ct = default);
        Task BulkInsertAsync(IEnumerable<HistoricalProject> projects, CancellationToken ct = default);

    }
}
