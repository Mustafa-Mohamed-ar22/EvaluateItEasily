using EvaluateItEasily.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class JwtProvider(IOptions<JwtOptions> options)
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateAccessTaoken(ApplicationUser user, IList<string> roles)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.issuer,
                audience: _options.audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.expiresIn),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static RefreshToken GenerateRefreshToken() => new()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            GeneratedIn = DateTime.UtcNow,
            ExpiresIn = DateTime.UtcNow.AddDays(7)
        };
        
    }
}