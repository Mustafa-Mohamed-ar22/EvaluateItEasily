namespace EvaluateItEasily.Infrastructure.Errors
{
    public static class NotificationErrors
    {
        public static readonly Error NotificationNotFound = 
            new("Notification.NotFound", 
                "Notification not found.", 
                StatusCodes.Status404NotFound);
        public static readonly Error NotNotificationOwner = 
            new("Notification.NotOwner", 
                "You are not the owner of this notification.", 
                StatusCodes.Status403Forbidden);
    }
}