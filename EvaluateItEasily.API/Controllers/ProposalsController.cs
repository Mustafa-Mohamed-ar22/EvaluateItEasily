using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.DTO_s.Proposals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
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
        public async Task<ActionResult<IEnumerable<ProposalResponse>>> GetAll([FromQuery] string? status, CancellationToken ct)
        {
            var result = await _proposalService.GetAllAsync(status,ct);
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
        public async Task<ActionResult<ProposalResponse>> Create([FromBody] CreateProposalRequest request,CancellationToken ct)
        {
            var result = await _proposalService.CreateAsync(request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<ProposalResponse>> Update(int id, [FromBody] UpdateProposalRequest request, CancellationToken ct)
        {
            var result = await _proposalService.UpdateAsync(id, request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{id}/download")]
        [Authorize(Roles = "Admin,Committee,Student")]
        public async Task<ActionResult<string>> Download(int id, CancellationToken ct)
        {
            var result = await _proposalService.DownloadProposalAsync(id, ct);

            if (result.IsFailure)
                return result.ToProblem();

            return Ok(new { downloadUrl = result.Data.Item2,previewUrl=result.Data.Item1 });
        }
    }
}