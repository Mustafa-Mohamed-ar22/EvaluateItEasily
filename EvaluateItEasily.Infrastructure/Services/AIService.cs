using EvaluateItEasily.Core.DTO_s.Evaluations;
using System.Net.Http.Json;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class AIService : IAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AISimilarityResponse?> CallAIApiAsync(AISimilarityRequest request, CancellationToken ct)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("AI_API");
                var response = await client.PostAsJsonAsync($"api/similarity", request, ct);

                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<AISimilarityResponse>(cancellationToken: ct);
            }
            catch
            {
                return null;
            }
        }
    }
}
