namespace EvaluateItEasily.Core.DTO_s
{
    public record FileDownloadResponse(
    Stream FileStream,     
    string ContentType,
    string FileName
);
}