
using System.Text.Json.Serialization;

namespace EvaluateItEasily.Core.DTO_s.Evaluations
{
    public record AISimilarityRequest
    (
        [property: JsonPropertyName("abstract")] string Abstract,
        [property: JsonPropertyName("top_k")] int TopK
        );
}
