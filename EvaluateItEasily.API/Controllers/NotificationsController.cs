// EvaluateItEasily.API/Controllers/NotificationsController.cs
using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
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
        public async Task<IActionResult> GetNotifications()
        {
            var result = await _notificationService.GetNotificationsForUserAsync();
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPatch("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notificationId);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var result = await _notificationService.MarkAllNotificationsAsReadAsync();
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
