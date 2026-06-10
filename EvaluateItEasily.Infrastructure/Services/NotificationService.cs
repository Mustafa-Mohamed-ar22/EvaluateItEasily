using EvaluateItEasily.Core.DTO_s.Notifications;
namespace EvaluateItEasily.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cacheService;
        private string GetCacheKey(string userId) => $"notifications:{userId}";
        public NotificationService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _cacheService = cacheService;
        }
        public async Task<Result<IEnumerable<NotificationResponse>>> GetNotificationsForUserAsync(CancellationToken ct = default)
        {
            var userId = _currentUserService.GetUserId();
            var cacheKey = GetCacheKey(userId!);

            var cachedNotifications = await _cacheService.GetAsync<IEnumerable<NotificationResponse>>(cacheKey, ct);

            if (cachedNotifications is not null)
            {
                return Result.Success(cachedNotifications);
            }
            var notifications = await _unitOfWork.Notifications.GetNotificationsForUserAsync(userId!);
            var notificationResponses = notifications.Adapt<IEnumerable<NotificationResponse>>();

            await _cacheService.SetAsync(cacheKey, notificationResponses, ct);

            return Result.Success(notificationResponses);
        }

        public async Task<Result> MarkAllNotificationsAsReadAsync(CancellationToken ct = default)
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

            await _unitOfWork.complete(ct);
            await _cacheService.RemoveAsync(GetCacheKey(userId!), ct);
            return Result.Success();
        }

        public async Task<Result> MarkNotificationAsReadAsync(int notificationId, CancellationToken ct = default)
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
                await _cacheService.RemoveAsync(GetCacheKey(userId!), ct);
            }
            return Result.Success();
        }
    }
}