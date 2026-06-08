namespace EvaluateItEasily.Core.DTO_s
{
    public record ArchiveRequest
    (
        string AcademicYear
    );
    public record PaginationRequest(int Page = 1, int PageSize = 10);
}
