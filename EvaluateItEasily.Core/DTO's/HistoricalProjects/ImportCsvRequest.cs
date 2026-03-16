using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.DTO_s
{
    public record ImportCsvRequest(IFormFile File);
}
