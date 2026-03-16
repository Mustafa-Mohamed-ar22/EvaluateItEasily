using EvaluateItEasily.Core.Entities;


namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IEvaluationRepository : IGenericRepository<Evaluation>
    {
        Task<Evaluation?> GetWithResultsAsync(int proposalId, CancellationToken ct = default);
        public Task<IEnumerable<Evaluation>> GetAllWithResultsAsync(CancellationToken ct = default);
    }
}
