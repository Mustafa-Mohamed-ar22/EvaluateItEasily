using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.SupervisorAssignments;
using EvaluateItEasily.Core.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EvaluateItEasily.API.Extensions;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SupervisorAssignmentsController : ControllerBase
    {
        private readonly ISupervisorAssignmentService _supervisorAssignmentService;

        public SupervisorAssignmentsController(ISupervisorAssignmentService supervisorAssignmentService)
        {
            _supervisorAssignmentService = supervisorAssignmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SupervisorAssignmentResponse>>> GetAll(CancellationToken ct)
        {
            var result = await _supervisorAssignmentService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SupervisorAssignmentResponse>> GetById(int id, CancellationToken ct)
        {
            var result = await _supervisorAssignmentService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("supervisor/my-assignments")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult<IEnumerable<SupervisorAssignmentResponse>>> GetMyAssignments(CancellationToken ct)
        {
            var result = await _supervisorAssignmentService.GetMyAssignmentsAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<SupervisorAssignmentResponse>> Create([FromBody] CreateSupervisorAssignmentRequest request, CancellationToken ct)
        {
            var result = await _supervisorAssignmentService.CreateAsync(request,ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
    }
}
