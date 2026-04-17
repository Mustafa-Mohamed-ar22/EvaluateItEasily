namespace EvaluateItEasily.Core.DTO_s.SubmissionPeriod
{
    public record SubmissionPeriodResponse(
    int Id,
    string Title,
    DateTime StartDate,
    DateTime EndDate,
    bool IsActive,
    bool IsOpen,             // ← computed live — is it open RIGHT NOW?
    DateTime CreatedOn,
    string CreatedByName
);
}
