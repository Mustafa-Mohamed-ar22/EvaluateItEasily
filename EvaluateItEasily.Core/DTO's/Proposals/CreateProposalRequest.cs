using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.DTO_s
{
    public record CreateProposalRequest
    (
        string Title,
        string Abstract,
        IFormFile ProposalFile 
        );
}