using EvaluateItEasily.Core.DTO_s.HistoricalProjects;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IHistoricalProjectService
    {
        Task<Result<IEnumerable<HistoricalProjectResponse>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<HistoricalProjectResponse>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Result<int>> ImportCsvAsync(IFormFile file, CancellationToken ct = default);
        Task<Result<int>> ArchiveAcceptedProposalsAsync(ArchiveRequest request, CancellationToken ct = default);
    }
}
