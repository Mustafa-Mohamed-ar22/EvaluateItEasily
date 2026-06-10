using EvaluateItEasily.Core.DTO_s.Users;
using FluentValidation;

namespace EvaluateItEasily.Infrastructure.Validators
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        private static readonly string[] AllowedRoles =
            ["Supervisor", "Committee", "Admin", "TechnicalAssistant", "Student"];
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one digit");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(r => AllowedRoles.Contains(r))
                .WithMessage
                ("Role must be Supervisor, Committee, Admin , TechnicalAssistant or Student");
        }
    }
}
