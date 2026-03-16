namespace EvaluateItEasily.Core.DTO_s.HistoricalProjects
{
    public record HistoricalProjectResponse
    (
        int Id,
        string Name,
        string Abstract,
        string GroupName,
        string AcademicYear,
        DateTime ArchivedAt,
        int? ProposalId
    );
}
