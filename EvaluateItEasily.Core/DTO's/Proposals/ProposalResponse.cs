namespace EvaluateItEasily.Core.DTO_s.Proposals
{
    public record ProposalResponse
    (
        int Id,
        string Title,
        string Abstract,
        string DownloadUrl,
        string Status,
        DateTime SubmittedAt,
        int GroupId,
        string GroupName,
        string LeaderName,
        int MembersCount,
        string FileName,
        string ContentType,
        string Domain
        );
}
