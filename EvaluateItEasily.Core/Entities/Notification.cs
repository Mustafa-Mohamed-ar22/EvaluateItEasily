using EvaluateItEasily.Core.Enums;

namespace EvaluateItEasily.Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;        
        public ApplicationUser User { get; set; } = default!;

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; } = false;
        public NotificationType Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
