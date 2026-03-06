using System.ComponentModel.DataAnnotations;

namespace EvaluateItEasily.Infrastructure.Options
{
    public class JwtOptions
    {
        public static string SectionName = "Jwt";
        [Required]
        public string key { get; init; } = string.Empty;
        [Required]
        public string issuer { get; init; } = string.Empty;
        [Required]
        public string audience { get; init; } = string.Empty;
        [Range(1, int.MaxValue)]
        public int expiresIn { get; init; }
    }
}
