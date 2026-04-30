using EvaluateItEasily.Core.DTO_s.Files;
using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IFileService
    {
        Task<Result<PresignedUploadResponse>> GenerateUploadUrlAsync(
            string fileName,
            CancellationToken ct = default);

        Task<Result<string>> GenerateDownloadUrlAsync(
            string storedFileName,
            CancellationToken ct = default);

        Task<Result> DeleteFileAsync(
            string storedFileName,
            CancellationToken ct = default);
    }
}
