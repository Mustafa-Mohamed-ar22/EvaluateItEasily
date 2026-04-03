using EvaluateItEasily.Core.DTO_s.Evaluations;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface IAIService
    {
        Task<AISimilarityResponse> CallAIApiAsync(AISimilarityRequest request,CancellationToken ct = default);
    }
}
