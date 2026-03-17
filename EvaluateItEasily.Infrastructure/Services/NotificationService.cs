// EvaluateItEasily.Infrastructure/Services/NotificationService.cs
using Mapster;
using EvaluateItEasily.Core;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Notifications;
using EvaluateItEasily.Core.Results;
using EvaluateItEasily.Infrastructure.Errors;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public NotificationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<IEnumerable<NotificationResponse>>> GetNotificationsForUserAsync()
        {
            var userId = _currentUserService.GetUserId();
            var notifications = await _unitOfWork.Notifications.GetNotificationsForUserAsync(userId!);
            var notificationResponses = notifications.Adapt<IEnumerable<NotificationResponse>>();
            return Result.Success(notificationResponses);
        }

        public async Task<Result> MarkAllNotificationsAsReadAsync()
        {
            var userId = _currentUserService.GetUserId();
            var notifications = await _unitOfWork.Notifications.GetNotificationsForUserAsync(userId!);

            foreach (var notification in notifications)
            {
                if (!notification.IsRead)
                {
                    notification.IsRead = true;
                }
            }

            await _unitOfWork.complete();
            return Result.Success();
        }

        public async Task<Result> MarkNotificationAsReadAsync(int notificationId)
        {
            var userId = _currentUserService.GetUserId();
            var notification = await _unitOfWork.Notifications.GetByIdAsync(notificationId);

            if (notification == null)
            {
                return Result.Failure(NotificationErrors.NotificationNotFound);
            }

            if (notification.UserId != userId)
            {
                return Result.Failure(NotificationErrors.NotNotificationOwner);
            }

            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _unitOfWork.complete();
            }

            return Result.Success();
        }
    }
}
