using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _groupService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _groupService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("my-group")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyGroup(CancellationToken ct)
        {
            var result = await _groupService.GetMyGroupAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Create([FromBody] CreateGroupRequest request, CancellationToken ct)
        {
            var result = await _groupService.CreateAsync(request,ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpPost("{groupId}/members")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> AddMember([FromRoute]int groupId, [FromBody] AddMemberRequest request,CancellationToken ct)
        {
            var result = await _groupService.AddMemberAsync(groupId, request,ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpDelete("{groupId}/members/{studentId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RemoveMember(int groupId, string studentId,CancellationToken ct)
        {
            var result = await _groupService.RemoveMemberAsync(groupId, studentId,ct);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }

        [HttpGet("available-students")]
        [Authorize(Roles = "Student,Admin,Committee")]
        public async Task<IActionResult> GetAvailableStudents(CancellationToken ct)
        {
            var result = await _groupService.GetAvailableStudentsAsync( ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
    }
}