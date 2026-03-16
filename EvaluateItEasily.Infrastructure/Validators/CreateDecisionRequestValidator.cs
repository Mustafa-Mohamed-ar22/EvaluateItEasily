using EvaluateItEasily.Core.DTO_s.Decisions;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class CreateDecisionRequestValidator : AbstractValidator<CreateDecisionRequest>
    {
        private static readonly string[] AllowedDecisions =
            ["Accepted", "Rejected", "RevisionRequested"];

        public CreateDecisionRequestValidator()
        {
            RuleFor(x => x.DecisionType)
                .NotEmpty().WithMessage("Decision type is required")
                .Must(d => AllowedDecisions.Contains(d))
                .WithMessage("Decision type must be Accepted, Rejected, or RevisionRequested");

            RuleFor(x => x.FeedbackComment)
                .NotEmpty().WithMessage("Feedback comment is required")
                .MaximumLength(2000).WithMessage("Feedback cannot exceed 2000 characters");
        }
    }
}
