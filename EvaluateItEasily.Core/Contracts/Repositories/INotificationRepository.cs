// EvaluateItEasily.Core/Contracts/Repositories/INotificationRepository.cs
using EvaluateItEasily.Core.Entities;

namespace EvaluateItEasily.Core.Contracts.Repositories
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId);
    }
}
