// EvaluateItEasily.Infrastructure/Repositories/NotificationRepository.cs
using EvaluateItEasily.Core.Contracts.Repositories;
using EvaluateItEasily.Core.Entities;
using EvaluateItEasily.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}
