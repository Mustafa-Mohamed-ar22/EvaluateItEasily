using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Core.Enums;

namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface IDecisionRepository : IGenericRepository<Decision>
    {
        Task<Decision?> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
        Task<IEnumerable<Decision>> GetByDecisionTypeAsync(DecisionType decisionType, CancellationToken ct = default);

    }
}
