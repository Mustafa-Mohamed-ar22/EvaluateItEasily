
namespace EvaluateItEasily.Infrastructure.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId, CancellationToken ct = default)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync(ct);
        }
        public async Task AddRangeAsync(IEnumerable<Notification> notifications, CancellationToken ct = default)
        {
            await _context.Notifications.AddRangeAsync(notifications, ct);
        }
    }
}
