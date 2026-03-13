using EvaluateItEasily.Infrastructure.Settings;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvaluateItEasily.Infrastructure.Validators.Common
{
    public class ValidateFileContent : AbstractValidator<IFormFile>
    {
        public ValidateFileContent()
        {
            RuleFor(x => x)
            .Must((request, context) =>
            {
                BinaryReader binary = new(request.OpenReadStream());
                var bytes = binary.ReadBytes(4);

                var fileSequenceHex = BitConverter.ToString(bytes);

                if (FileSettings.AllowedFileExtensionSignatures.Equals(fileSequenceHex, StringComparison.OrdinalIgnoreCase))
                    return true;

                return false;
            })
            .WithMessage("Not allowed file content")
            .When(x => x is not null);
        }
    }
}