
using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.DTO_s
{
    public record UpdateProposalRequest
    (
        string Title,
        string Abstract,
        IFormFile ProposalFile
    );
}
