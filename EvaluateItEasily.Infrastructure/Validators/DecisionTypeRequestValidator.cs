using EvaluateItEasily.Core.DTO_s.Decisions;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class DecisionTypeRequestValidator : AbstractValidator<DecisionTypeRequest>
    {
        private static readonly string[] AllowedDecisions =
            ["Accepted", "Rejected", "RevisionRequested"];

        public DecisionTypeRequestValidator()
        {
            RuleFor(x => x.DecisionType)
                .NotEmpty().WithMessage("Decision type is required")
                .Must(d => AllowedDecisions.Contains(d))
                .WithMessage("Decision type must be Accepted, Rejected, or RevisionRequested");
        }
    }
}
