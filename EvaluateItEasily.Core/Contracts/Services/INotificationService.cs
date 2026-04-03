using EvaluateItEasily.Core.DTO_s.Notifications;
using EvaluateItEasily.Core.Results;

namespace EvaluateItEasily.Core.Contracts.Services
{
    public interface INotificationService
    {
        Task<Result<IEnumerable<NotificationResponse>>> GetNotificationsForUserAsync(CancellationToken ct = default);
        Task<Result> MarkNotificationAsReadAsync(int notificationId, CancellationToken ct = default);
        Task<Result> MarkAllNotificationsAsReadAsync(CancellationToken ct = default);
    }
}
