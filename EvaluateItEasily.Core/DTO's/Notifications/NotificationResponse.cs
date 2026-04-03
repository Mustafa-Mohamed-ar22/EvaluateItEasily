using EvaluateItEasily.Core.Enums;

namespace EvaluateItEasily.Core.DTO_s.Notifications
{
    public record NotificationResponse(
        int Id,
        string Title,
        string Message,
        bool IsRead,
        NotificationType Type,
        DateTime CreatedAt
    );
}
