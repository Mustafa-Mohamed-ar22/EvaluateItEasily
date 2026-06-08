using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Core.DTO_s
{
    public record CreateProposalRequest(
        string Title,
        string Abstract,
        string OriginalFileName,   
        string StoredFileName,     
        string ContentType       ,
        string Domain
    );
}