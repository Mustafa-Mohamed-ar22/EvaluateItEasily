using FluentValidation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using LoginRequest = EvaluateItEasily.Core.Auth.LoginRequest;
namespace EvaluateItEasily.Infrastructure.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
     .NotEmpty().WithMessage("Email or National ID is required")
     .Must(BeEmailOrEgyptianNationalId)
     .WithMessage("Must be a valid email or 14-digit Egyptian National ID");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
        private bool BeEmailOrEgyptianNationalId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (new EmailAddressAttribute().IsValid(value))
                return true;

            return Regex.IsMatch(value, @"^\d{14}$");
        }
    }
}
