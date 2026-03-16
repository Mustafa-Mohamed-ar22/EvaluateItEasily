
using System.Text.Json.Serialization;

namespace EvaluateItEasily.Core.DTO_s.Evaluations
{
    public record AISimilarProject
    (
        [property: JsonPropertyName("project_id")] int ProjectId,
        [property: JsonPropertyName("project_name")] string ProjectName,
        [property: JsonPropertyName("project_abstract")] string ProjectAbstract,
        [property: JsonPropertyName("project_date")] string ProjectDate,
        [property: JsonPropertyName("similarity_score")] float SimilarityScore
    );
}
