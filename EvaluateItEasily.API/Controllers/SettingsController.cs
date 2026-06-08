using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : ControllerBase
    {
        private readonly ISystemSettingService _systemSettingService;

        public SettingsController(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        [HttpGet("threshold")]
        public async Task<ActionResult<ThresholdResponse>> GetThreshold(CancellationToken ct)
        {
            var result = (await _systemSettingService.GetThresholdAsync(ct));
            return result.IsSuccess ? result.Data : result.ToProblem() ;
        }

        [HttpPut("threshold")]
        public async Task<ActionResult<ThresholdResponse>> SetThreshold([FromBody] SetThresholdRequest request,CancellationToken ct)
        {
            var result = (await _systemSettingService.SetThresholdAsync(request,ct));
            return result.IsSuccess ? result.Data : result.ToProblem();
        }
    }
}
