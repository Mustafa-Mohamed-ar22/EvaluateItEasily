// EvaluateItEasily.Core/Contracts/Services/INotificationService.cs
using EvaluateItEasily.Core.DTO_s.Notifications;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface INotificationService
    {
        Task<Result<IEnumerable<NotificationResponse>>> GetNotificationsForUserAsync();
        Task<Result> MarkNotificationAsReadAsync(int notificationId);
        Task<Result> MarkAllNotificationsAsReadAsync();
    }
}
