using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IFileService
    {
        Task<Result<(string, string)>> SaveFileAsync(IFormFile file, CancellationToken ct = default);
        void DeleteFile(string fileUrl);
        Task<Result<byte[]>> GetFileAsync(string relativeUrl, CancellationToken ct = default);

    }
}
