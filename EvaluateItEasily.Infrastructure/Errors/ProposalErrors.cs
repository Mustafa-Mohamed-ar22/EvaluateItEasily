using Microsoft.AspNetCore.Http;

namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class ProposalErrors
    {
        public static readonly Error NotFound = new(
            "Proposal.NotFound",
            "Proposal was not found",
            StatusCodes.Status404NotFound);

        public static readonly Error AlreadySubmitted = new(
            "Proposal.AlreadySubmitted",
            "Your group already has a submitted proposal",
            StatusCodes.Status409Conflict);

        public static readonly Error NotLeader = new(
            "Proposal.NotLeader",
            "Only the group leader can submit or update a proposal",
            StatusCodes.Status401Unauthorized);

        public static readonly Error NoGroup = new(
            "Proposal.NoGroup",
            "You must be in a group before submitting a proposal",
            StatusCodes.Status400BadRequest);

        public static readonly Error CannotUpdate = new(
            "Proposal.CannotUpdate",
            "Proposal can only be updated when status is Pending or RevisionRequested",
            StatusCodes.Status400BadRequest);
        public static readonly Error CannotUpload= new(
           "Proposal.CannotUpdate",
           "Operation Failed",
           StatusCodes.Status500InternalServerError);

        public static readonly Error NoProposal = new(
            "Proposal.NoProposal",
            "Your group has not submitted a proposal yet",
            StatusCodes.Status404NotFound);
        public static readonly Error FileNotFound = new(
     "Proposal.FileNotFound",
    "Proposal file was not found on the server",
        StatusCodes.Status404NotFound);

        public static readonly Error CannotDownload = new(
            "Proposal.CannotDownload",
            "You are not allowed to download this proposal",
            StatusCodes.Status401Unauthorized);
        public static readonly Error InvalidStatus = new(
    "Proposal.InvalidStatus",
    "Invalid status value. Valid values are: Pending, UnderReview, Accepted, Rejected, RevisionRequested",
    StatusCodes.Status400BadRequest);
    }
}
