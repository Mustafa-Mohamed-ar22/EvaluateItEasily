using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Evaluations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationsController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationsController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpPost("{proposalId}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<EvaluationResponse>> TriggerEvaluation(int proposalId,CancellationToken ct)
        {
            var result = await _evaluationService.TriggerEvaluationAsync(proposalId, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{proposalId}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<EvaluationResponse>> GetByProposalId(int proposalId,CancellationToken ct)
        {
            var result = await _evaluationService.GetByProposalIdAsync(proposalId, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpGet("")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<IEnumerable<EvaluationResponse>>> GetAllEvaluations(CancellationToken ct)
        {
            var result = await _evaluationService.GetAllEvaluationsAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
    }
}
