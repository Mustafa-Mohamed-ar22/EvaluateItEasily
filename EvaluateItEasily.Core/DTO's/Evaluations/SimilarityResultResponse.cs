namespace EvaluateItEasily.Core.DTO_s
{
    public record SimilarityResultResponse
    (
        int Rank,
        float SimilarityScore,
        string MatchedProjectName,
        string MatchedProjectAbstract,
        string MatchedProjectYear
    );
}
