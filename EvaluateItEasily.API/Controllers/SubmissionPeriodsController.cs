using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.SubmissionPeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubmissionPeriodsController : ControllerBase
    {
        private readonly ISubmissionPeriodService _submissionPeriodService;

        public SubmissionPeriodsController(ISubmissionPeriodService submissionPeriodService)
        {
            _submissionPeriodService = submissionPeriodService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SubmissionPeriodResponse>>> GetAll(CancellationToken ct)
        {
            var result = await _submissionPeriodService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("current")]
        [Authorize(Roles = "Admin,Committee,Student")]
        public async Task<ActionResult<SubmissionPeriodResponse>> GetCurrent(CancellationToken ct)
        {
            var result = await _submissionPeriodService.GetCurrentAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubmissionPeriodResponse>> Create([FromBody] SetSubmissionPeriodRequest request,CancellationToken ct)
        {
            var result = await _submissionPeriodService.CreateAsync(request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SubmissionPeriodResponse>> Update(int id,[FromBody] SetSubmissionPeriodRequest request,CancellationToken ct)
        {
            var result = await _submissionPeriodService.UpdateAsync(id, request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}/toggle-active")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ToggleActive(int id, CancellationToken ct)
        {
            var result = await _submissionPeriodService.ToggleActiveAsync(id, ct);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}