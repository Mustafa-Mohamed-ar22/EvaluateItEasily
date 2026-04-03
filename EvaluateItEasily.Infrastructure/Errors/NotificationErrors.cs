
namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class NotificationErrors
    {
        public static readonly Error NotificationNotFound = new("Notification.NotFound", "Notification not found.", 404);
            public static readonly Error NotNotificationOwner = new("Notification.NotOwner", "You are not the owner of this notification.", 403);
    }
}
