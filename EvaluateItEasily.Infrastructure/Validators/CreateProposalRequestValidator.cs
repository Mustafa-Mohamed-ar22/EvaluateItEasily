using EvaluateItEasily.Infrastructure.Validators.Common;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    //public class CreateProposalRequestValidator : AbstractValidator<CreateProposalRequest>
    //{
    //    public CreateProposalRequestValidator()
    //    {
    //        RuleFor(x => x.Title)
    //            .NotEmpty().WithMessage("Title is required")
    //            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

    //        RuleFor(x => x.Abstract)
    //            .NotEmpty().WithMessage("Abstract is required")
    //            .MinimumLength(20).WithMessage("Abstract must be at least 20 characters")
    //            .MaximumLength(5000).WithMessage("Abstract cannot exceed 5000 characters");

    //        RuleFor(x => x.ProposalFile)
    //            .NotNull().WithMessage("Proposal file is required");
    //        RuleFor(x => x.ProposalFile).SetValidator(new ValidateFileSize())
    //            .SetValidator(new ValidateFileContent());

    //        RuleFor(x => x.ProposalFile.FileName)
    //            .SetValidator(new ValidateFileName())
    //            .When(x => x.ProposalFile is not null);
    //    }

    //}

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

            // Original file name (user-provided display name)
            RuleFor(x => x.OriginalFileName)
                  .SetValidator(new ValidateFileName())
                  .When(x => x.OriginalFileName is not null);

            // Stored file name (from presigned upload - usually a key/path)
            RuleFor(x => x.StoredFileName)
                .NotEmpty().WithMessage("Stored file name is required")
                .MaximumLength(500).WithMessage("Stored file name cannot exceed 500 characters");

            // Content type validation
            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("Content type is required")
                .Must(BeAValidPdfContentType).WithMessage("Only PDF files are allowed");
        }

        private bool BeAValidPdfContentType(string contentType)
        {
            return contentType == "application/pdf";
        }
    }
}
