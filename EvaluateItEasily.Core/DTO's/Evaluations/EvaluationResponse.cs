namespace EvaluateItEasily.Core.DTO_s.Evaluations
{
    public record EvaluationResponse
    (
        int Id,
    int ProposalId,
    string ProposalTitle,
    string EvaluatedByName,
    string AIStatus,
    float MaxSimilarityScore,
    DateTime EvaluatedAt,
    IEnumerable<SimilarityResultResponse> SimilarityResults
        );
}
