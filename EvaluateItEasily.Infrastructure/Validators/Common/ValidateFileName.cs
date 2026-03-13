using FluentValidation;
namespace EvaluateItEasily.Infrastructure.Validators.Common
{
    public class ValidateFileName : AbstractValidator<string>
    {
        public ValidateFileName()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("File name cannot be empty")
                .Matches(@"^[A-Za-z0-9_. ]+$")
                .WithMessage("File name can only contain letters, numbers, spaces, '_' and '.'")
                .Must(x => !string.IsNullOrWhiteSpace(x?.Trim('.')))
                .WithMessage("File name cannot consist only of dots");
        }
    }
}
