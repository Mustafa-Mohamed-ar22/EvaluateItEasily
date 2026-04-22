using EvaluateItEasily.Core.Entities;


namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface ISimilarityResultRepository : IGenericRepository<SimilarityResult>
    {
        Task DeleteByEvaluationIdAsync(int evaluationId, CancellationToken ct = default);
    }
}
