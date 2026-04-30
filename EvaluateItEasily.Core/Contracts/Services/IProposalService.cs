using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.DTO_s.Proposals;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IProposalService
    {
        Task<Result<IEnumerable<ProposalResponse>>> GetAllAsync(string? status = null, CancellationToken ct = default);
        Task<Result<ProposalResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<ProposalResponse>> GetMyProposalAsync(CancellationToken ct = default);
        Task<Result<ProposalResponse>> CreateAsync(CreateProposalRequest request, CancellationToken ct = default);
        Task<Result<(string,string)>> DownloadProposalAsync(int id, CancellationToken ct = default);
        Task<Result<ProposalResponse>> UpdateAsync(int id, UpdateProposalRequest request, CancellationToken ct = default);
    }
}
