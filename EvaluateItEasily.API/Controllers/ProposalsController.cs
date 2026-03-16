using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.DTO_s.Proposals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProposalsController : ControllerBase
    {
        private readonly IProposalService _proposalService;
        public ProposalsController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<IEnumerable<ProposalResponse>>> GetAll(CancellationToken ct)
        {
            var result = await _proposalService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<ProposalResponse>> GetById(int id, CancellationToken ct) 
        {
            var result = await _proposalService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
            

        [HttpGet("my-proposal")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ProposalResponse>> GetMyProposal(CancellationToken ct)
        {
            var result = await _proposalService.GetMyProposalAsync( ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ProposalResponse>> Create([FromForm] CreateProposalRequest request,CancellationToken ct)
        {
            var result = await _proposalService.CreateAsync(request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ProposalResponse>> Update(int id,[FromForm] UpdateProposalRequest request,CancellationToken ct)
        {
            var result = await _proposalService.UpdateAsync(id,request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{id}/download")]
        [Authorize(Roles = "Admin,Committee,Student")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Download(int id, CancellationToken ct)
        {
            var result = await _proposalService.DownloadProposalAsync(id, ct);
            return result.IsSuccess ? result.ToFileResult() : result.ToProblem();
        }
    }
}