namespace EvaluateItEasily.Core.DTO_s.Files
{
    public record PresignedUploadResponse(
    string UploadUrl,
    string StoredFileName
);
}
