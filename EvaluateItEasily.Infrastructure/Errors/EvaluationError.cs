using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Infrastructure.Errors
{
    public class EvaluationError
    {
        public static readonly Error NotFound = new(
        "Evaluation.NotFound",
        "Evaluation was not found",
        StatusCodes.Status404NotFound);

        public static readonly Error AlreadyEvaluated = new(
            "Evaluation.AlreadyEvaluated",
            "This proposal has already been evaluated",
            StatusCodes.Status409Conflict);

        public static readonly Error ProposalNotFound = new(
            "Evaluation.ProposalNotFound",
            "Proposal was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error AIServiceFailed = new(
            "Evaluation.AIServiceFailed",
            "AI similarity service failed to respond ... try after some seconds",
            StatusCodes.Status500InternalServerError);

        public static readonly Error ProposalNotPending = new(
            "Evaluation.ProposalNotPending",
            "Only pending proposals can be evaluated",
            StatusCodes.Status409Conflict);
        public static readonly Error ProposalNotBelongToStudent = new(
     "Evaluation.ProposalNotBelongToStudent",
     "This proposal does not belong to your group",
     StatusCodes.Status401Unauthorized);
    }
}
