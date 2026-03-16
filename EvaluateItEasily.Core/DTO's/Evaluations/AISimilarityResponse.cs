using System.Text.Json.Serialization;

namespace EvaluateItEasily.Core.DTO_s.Evaluations
{
    public record AISimilarityResponse
    (
        [property: JsonPropertyName("input_abstract")] string InputAbstract,
        [property: JsonPropertyName("top_k")] int TopK,
        [property: JsonPropertyName("results")] List<AISimilarProject> Results
        );
}
