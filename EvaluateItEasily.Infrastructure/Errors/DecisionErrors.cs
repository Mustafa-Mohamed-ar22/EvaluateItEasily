namespace EvaluateItEasily.Infrastructure.Errors
{
    public class DecisionErrors
    {
        public static readonly Error NotFound = new(
               "Decision.NotFound",
               "Decision was not found",
               StatusCodes.Status404NotFound);

        public static readonly Error AlreadyDecided = new(
                "Decision.AlreadyDecided",
                "A decision has already been made for this proposal",
                StatusCodes.Status409Conflict);

        public static readonly Error ProposalNotEvaluated = new(
                "Decision.ProposalNotEvaluated",
                "Proposal must be evaluated before a decision can be made",
                StatusCodes.Status400BadRequest);

        public static readonly Error InvalidDecisionType = new(
                "Decision.InvalidDecisionType",
                "Decision type must be Accepted, Rejected, or RevisionRequested",
                StatusCodes.Status400BadRequest);

        public static readonly Error ProposalNotFound = new(
                "Decision.ProposalNotFound",
                "Proposal was not found",
                StatusCodes.Status404NotFound);
    }
}
