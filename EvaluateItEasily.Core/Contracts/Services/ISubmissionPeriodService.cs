using EvaluateItEasily.Core.DTO_s.SubmissionPeriod;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface ISubmissionPeriodService
    {
        Task<Result<IEnumerable<SubmissionPeriodResponse>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<SubmissionPeriodResponse>> GetCurrentAsync(CancellationToken ct = default);
        Task<Result<SubmissionPeriodResponse>> CreateAsync(SetSubmissionPeriodRequest request, CancellationToken ct = default);
        Task<Result<SubmissionPeriodResponse>> UpdateAsync(int id, SetSubmissionPeriodRequest request, CancellationToken ct = default);
        Task<Result> ToggleActiveAsync(int id, CancellationToken ct = default);
        Task<Result> ValidateIsOpenAsync(CancellationToken ct = default);  // ← I will use it internally in ProposalService
    }
}
