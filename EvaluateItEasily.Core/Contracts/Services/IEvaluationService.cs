using EvaluateItEasily.Core.DTO_s.Evaluations;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IEvaluationService
    {
        Task<Result<EvaluationResponse>> TriggerEvaluationAsync(int proposalId, CancellationToken ct = default);
        Task<Result<EvaluationResponse>> GetByProposalIdAsync(int proposalId, CancellationToken ct = default);
        Task<Result<IEnumerable<EvaluationResponse>>> GetAllEvaluationsAsync(CancellationToken ct = default);
    }
}