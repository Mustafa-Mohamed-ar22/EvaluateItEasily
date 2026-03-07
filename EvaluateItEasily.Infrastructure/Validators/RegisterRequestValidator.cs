using EvaluateItEasily.Infrastructure.Data;
using FluentValidation;
using RegisterRequest = EvaluateItEasily.Core.Auth.RegisterRequest;


namespace EvaluateItEasily.Infrastructure.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        private readonly AppDbContext _context;

        public RegisterRequestValidator(AppDbContext context)
        {
            _context = context;
        }

        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .Must(x=>_context.Users.All(y=>y.Email!=x)).WithMessage("We Already have this email");

            

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
