namespace EvaluateItEasily.Core.DTO_s
{
    public record DecisionResponse
    (
        int Id,
        int ProposalId,
        string ProposalTitle,
        string DecidedByName,
        string DecisionType,
        string FeedbackComment,
        DateTime DecidedAt
    );
}