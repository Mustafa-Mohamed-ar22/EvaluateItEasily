using Microsoft.EntityFrameworkCore;

namespace EvaluateItEasily.Core.Entities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
        public DateTime GeneratedIn { get; set; }
        public DateTime? RevokedIn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresIn;
        public bool IsActive => !IsExpired && RevokedIn is null;
    }
}
