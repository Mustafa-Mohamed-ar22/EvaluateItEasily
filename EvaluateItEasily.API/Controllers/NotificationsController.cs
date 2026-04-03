using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetNotifications(CancellationToken ct = default)
        {
            var result = await _notificationService.GetNotificationsForUserAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPatch("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId, CancellationToken ct = default)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notificationId,ct);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken ct = default)
        {
            var result = await _notificationService.MarkAllNotificationsAsReadAsync(ct);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
