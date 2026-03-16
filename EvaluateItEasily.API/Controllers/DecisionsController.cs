using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.DTO_s.Decisions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DecisionsController : ControllerBase
    {
        private readonly IDecisionService _decisionService;

        public DecisionsController(IDecisionService decisionService)
        {
            _decisionService = decisionService;
        }

        [HttpPost("{proposalId}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<DecisionResponse>> Create(int proposalId,[FromBody] CreateDecisionRequest request,CancellationToken ct)
        {
            var result = await _decisionService.CreateAsync(proposalId,request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{proposalId}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<DecisionResponse>> GetByProposalId(int proposalId,CancellationToken ct)
        {
            var result = await _decisionService.GetByProposalIdAsync(proposalId, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpGet("type/{decisionType}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<IEnumerable<DecisionResponse>>> GetByDecisionType([FromRoute]DecisionTypeRequest request,CancellationToken ct)
        {
            var result = await _decisionService.GetByDecisionTypeAsync(request.DecisionType, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();  
        }
    }
}
