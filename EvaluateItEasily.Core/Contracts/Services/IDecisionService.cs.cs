using EvaluateItEasily.Core.DTO_s.Decisions;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IDecisionService
    {
        Task<Result<DecisionResponse>> CreateAsync(int proposalId, CreateDecisionRequest request, CancellationToken ct = default);
        Task<Result<DecisionResponse>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
        public Task<Result<IEnumerable<DecisionResponse>>> GetByDecisionTypeAsync(string decisionType, CancellationToken ct = default);
    }
}
