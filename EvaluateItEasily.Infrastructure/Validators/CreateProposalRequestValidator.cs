using EvaluateItEasily.Infrastructure.Validators.Common;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class CreateProposalRequestValidator : AbstractValidator<CreateProposalRequest>
    {
        public CreateProposalRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Abstract)
                .NotEmpty().WithMessage("Abstract is required")
                .MinimumLength(20).WithMessage("Abstract must be at least 20 characters")
                .MaximumLength(5000).WithMessage("Abstract cannot exceed 5000 characters");

            RuleFor(x => x.ProposalFile)
                .NotNull().WithMessage("Proposal file is required");
            RuleFor(x => x.ProposalFile).SetValidator(new ValidateFileSize())
                .SetValidator(new ValidateFileContent());

            RuleFor(x => x.ProposalFile.FileName)
                .SetValidator(new ValidateFileName())
                .When(x => x.ProposalFile is not null);
        }
    }
}
