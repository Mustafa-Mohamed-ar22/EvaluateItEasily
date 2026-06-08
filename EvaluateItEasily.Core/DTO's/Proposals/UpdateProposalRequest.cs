namespace EvaluateItEasily.Core.DTO_s
{
    public record UpdateProposalRequest(
    string Title,
    string Abstract,
    string OriginalFileName,
    string StoredFileName,
    string ContentType,
    string Domain
    );
}