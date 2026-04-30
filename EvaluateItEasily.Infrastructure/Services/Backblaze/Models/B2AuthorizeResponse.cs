using System.Text.Json.Serialization;

namespace EvaluateItEasily.Infrastructure.Services.Backblaze.Models
{
    internal record B2AuthorizeResponse(
    [property: JsonPropertyName("authorizationToken")] string AuthorizationToken,
    [property: JsonPropertyName("accountId")] string AccountId,
    [property: JsonPropertyName("apiInfo")] B2ApiInfo ApiInfo
)
    {
        public string ApiUrl => ApiInfo.StorageApi.ApiUrl;
        public string DownloadUrl => ApiInfo.StorageApi.DownloadUrl;
    }

    internal record B2ApiInfo(
        [property: JsonPropertyName("storageApi")] B2StorageApi StorageApi
    );

    internal record B2StorageApi(
        [property: JsonPropertyName("apiUrl")] string ApiUrl,
        [property: JsonPropertyName("downloadUrl")] string DownloadUrl
    );

    // These stay the same
    internal record B2UploadUrlResponse(
        [property: JsonPropertyName("uploadUrl")] string UploadUrl,
        [property: JsonPropertyName("authorizationToken")] string AuthorizationToken
    );

    internal record B2UploadFileResponse(
        [property: JsonPropertyName("fileId")] string FileId,
        [property: JsonPropertyName("fileName")] string FileName
    );

    internal record B2DownloadAuthResponse(
        [property: JsonPropertyName("authorizationToken")] string AuthorizationToken
    );

    internal record B2DeleteResponse(
        [property: JsonPropertyName("fileId")] string FileId,
        [property: JsonPropertyName("fileName")] string FileName
    );
}
