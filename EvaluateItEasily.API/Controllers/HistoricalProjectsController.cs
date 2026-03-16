using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.DTO_s.HistoricalProjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricalProjectsController : ControllerBase
    {
        private readonly IHistoricalProjectService _historicalProjectService;

        public HistoricalProjectsController(IHistoricalProjectService historicalProjectService)
        {
            _historicalProjectService = historicalProjectService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Committee,Supervisor")]
        public async Task<ActionResult<IEnumerable<HistoricalProjectResponse>>> GetAll(CancellationToken ct)
        {
            var result = await _historicalProjectService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Committee,Supervisor,Student")]
        public async Task<ActionResult<HistoricalProjectResponse>> GetById(int id, CancellationToken ct)
        {
            var result = await _historicalProjectService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost("import-csv")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> ImportCsv([FromForm]ImportCsvRequest request,CancellationToken ct)
        {
            var result = await _historicalProjectService.ImportCsvAsync(request.File, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpPost("archive")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<int>> Archive([FromBody] ArchiveRequest request,CancellationToken ct)
        {
            var result = await _historicalProjectService.ArchiveAcceptedProposalsAsync(request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();   
        }
    }
}
