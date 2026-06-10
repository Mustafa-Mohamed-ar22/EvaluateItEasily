using EvaluateItEasily.Infrastructure.Validators.Common;
using FluentValidation;
namespace EvaluateItEasily.Infrastructure.Validators
{
    public class ImportCsvRequestValidator : AbstractValidator<ImportCsvRequest>
    {
        public ImportCsvRequestValidator()
        {

            RuleFor(x => x.File)
                .SetValidator(new ValidateFileSize())
                .WithMessage("File Size Can't exceed 10 MB")
                .When(x=>x.File is not null);

            RuleFor(x => x.File)
             .Must(file =>
             {
                 var ext = Path.GetExtension(file.FileName);
                 if (!ext.Equals(".csv", StringComparison.OrdinalIgnoreCase))
                     return false;

                 var allowedMimeTypes = new[] { "text/csv", "application/csv", "text/plain" };
                 if (!allowedMimeTypes.Contains(file.ContentType, StringComparer.OrdinalIgnoreCase))
                     return false;

                 using var stream = file.OpenReadStream();
                 using var reader = new StreamReader(stream);
                 var firstLine = reader.ReadLine();

                 return !string.IsNullOrWhiteSpace(firstLine)
                        && !firstLine.Any(c => c == '\0');
            })
                .WithMessage("File must be a valid CSV file.")
                .When(x => x.File != null);

        }

    }
}
