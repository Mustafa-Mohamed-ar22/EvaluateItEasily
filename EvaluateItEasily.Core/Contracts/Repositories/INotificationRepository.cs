using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct = default);
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId, CancellationToken ct = default);
    }
}
