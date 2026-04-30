using EvaluateItEasily.Infrastructure.Validators.Common;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class UpdateProposalRequestValidator : AbstractValidator<UpdateProposalRequest>
    {
        public UpdateProposalRequestValidator()
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
                .NotEmpty().WithMessage("Original file name is required")
                .MaximumLength(255).WithMessage("Original file name cannot exceed 255 characters")
                .Must(BeAValidFileName).WithMessage("Original file name is not valid");

            // Stored file name (from presigned upload - usually a key/path)
            RuleFor(x => x.StoredFileName)
                .NotEmpty().WithMessage("Stored file name is required")
                .MaximumLength(500).WithMessage("Stored file name cannot exceed 500 characters");

            // Content type validation
            RuleFor(x => x.ContentType)
                .NotEmpty().WithMessage("Content type is required")
                .Must(BeAValidPdfContentType).WithMessage("Only PDF files are allowed");
        }
        private bool BeAValidFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var invalidChars = Path.GetInvalidFileNameChars();
            return !fileName.Any(c => invalidChars.Contains(c));
        }

        private bool BeAValidPdfContentType(string contentType)
        {
            return contentType == "application/pdf";
        }
    }
}
