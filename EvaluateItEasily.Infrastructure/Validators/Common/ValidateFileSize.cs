using EvaluateItEasily.Infrastructure.Settings;
using FluentValidation;
namespace EvaluateItEasily.Infrastructure.Validators.Common
{
    public class ValidateFileSize : AbstractValidator<IFormFile>
    {
        public ValidateFileSize()
        {
            RuleFor(x => x)
                .Must((request, context) => request.Length <= FileSettings.MaxFileSizeInBytes)
                .WithMessage("Proposal File Size Can't Exceed 10 MB")
                .When(x => x is not null);
        }
    }
}
