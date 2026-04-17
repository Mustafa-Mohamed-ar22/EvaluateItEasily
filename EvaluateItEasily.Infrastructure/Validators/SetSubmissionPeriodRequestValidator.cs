using EvaluateItEasily.Core.DTO_s.SubmissionPeriod;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class SetSubmissionPeriodRequestValidator : AbstractValidator<SetSubmissionPeriodRequest>
    {
        public SetSubmissionPeriodRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");
        }
    }
}
