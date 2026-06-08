
namespace EvaluateItEasily.Core.DTO_s.Users
{
    public record ImportStudentsResponse(
    int TotalCount,
    int SuccessCount,
    int FailedCount,
    IEnumerable<string> FailedEntries  // names of failed students
);
}
