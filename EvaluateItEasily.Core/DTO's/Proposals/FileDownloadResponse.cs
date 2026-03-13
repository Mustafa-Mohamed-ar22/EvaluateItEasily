namespace EvaluateItEasily.Core.DTO_s
{
    public record FileDownloadResponse(
    byte[] FileBytes,
    string ContentType,
    string FileName
);
}