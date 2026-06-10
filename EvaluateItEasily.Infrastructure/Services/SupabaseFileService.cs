using EvaluateItEasily.Core.DTO_s.Files;
using EvaluateItEasily.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class SupabaseFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        private readonly SupabaseSettings _settings;
        private readonly ILogger<SupabaseFileService> _logger;

        private const string PdfExtension = ".pdf";

        public SupabaseFileService(
            HttpClient httpClient,
            IOptions<SupabaseSettings> settings,
            ILogger<SupabaseFileService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _logger = logger;
        }
        public async Task<Result<PresignedUploadResponse>> GenerateUploadUrlAsync(
            string fileName,
            CancellationToken ct = default)
        {
            try
            {
                // Validate extension
                var extension = Path.GetExtension(fileName).ToLowerInvariant();
                if (extension != PdfExtension)
                    return Result.Failure<PresignedUploadResponse>(FileErrors.InvalidExtension);

                var storedFileName = $"{Path.GetRandomFileName()}.pdf";
                var endpoint = $"{_settings.Url}/storage/v1/object/upload/sign/{_settings.BucketName}/{storedFileName}";

                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Add("Authorization", $"Bearer {_settings.ServiceRoleKey}");
                request.Headers.Add("apikey", _settings.ServiceRoleKey);

                request.Content = JsonContent.Create(new { });

                var response = await _httpClient.SendAsync(request, ct);
                var responseContent = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Supabase generate upload URL failed: {Content}", responseContent);
                    return Result.Failure<PresignedUploadResponse>(FileErrors.SaveFailed);
                }

                var json = JsonDocument.Parse(responseContent);
                var token = json.RootElement.GetProperty("token").GetString()!;
                var url = $"{_settings.Url}/storage/v1/object/upload/sign/{_settings.BucketName}/{storedFileName}?token={token}";

                return Result.Success(new PresignedUploadResponse(
                    UploadUrl: url,
                    StoredFileName: storedFileName
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Supabase generate upload URL failed: {Message}", ex.Message);
                return Result.Failure<PresignedUploadResponse>(FileErrors.SaveFailed);
            }
        }
        public async Task<Result<string>> GenerateDownloadUrlAsync(string storedFileName,CancellationToken ct = default)
        {
            try
            {
                var endpoint = $"{_settings.Url}/storage/v1/object/sign/{_settings.BucketName}/{storedFileName}";

                using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
                request.Headers.Add("Authorization", $"Bearer {_settings.ServiceRoleKey}");
                request.Headers.Add("apikey", _settings.ServiceRoleKey);

                request.Content = JsonContent.Create(new
                {
                    expiresIn = _settings.DownloadExpirySeconds
                });

                var response = await _httpClient.SendAsync(request, ct);
                var responseContent = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Supabase sign URL failed: {Content}", responseContent);
                    return Result.Failure<string>(FileErrors.FileNotFound);
                }

                var json = JsonDocument.Parse(responseContent);
                var signedPath = json.RootElement.GetProperty("signedURL").GetString()!;
                var fullUrl = $"{_settings.Url}/storage/v1{signedPath}";

                return Result.Success(fullUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Supabase generate download URL failed: {Message}", ex.Message);
                return Result.Failure<string>(FileErrors.FileNotFound);
            }
        }
        public async Task<Result> DeleteFileAsync(string storedFileName,CancellationToken ct = default)
        {
            try
            {
                var endpoint = $"{_settings.Url}/storage/v1/object/{_settings.BucketName}";

                using var request = new HttpRequestMessage(HttpMethod.Delete, endpoint);
                request.Headers.Add("Authorization", $"Bearer {_settings.ServiceRoleKey}");
                request.Headers.Add("apikey", _settings.ServiceRoleKey);

                request.Content = JsonContent.Create(new
                {
                    prefixes = new[] { storedFileName }
                });

                var response = await _httpClient.SendAsync(request, ct);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync(ct);
                    _logger.LogError("Supabase delete failed: {Content}", content);
                    return Result.Failure(FileErrors.DeleteFailed);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Supabase delete failed: {Message}", ex.Message);
                return Result.Failure(FileErrors.DeleteFailed);
            }
        }
    }
}
