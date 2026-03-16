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
        private readonly ICacheService _cacheService;

        public GroupsController(IGroupService groupService,ICacheService cacheService)
        {
            _groupService = groupService;
            _cacheService = cacheService;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<IEnumerable<GroupResponse>>> GetAll(CancellationToken ct)
        {
            var result = await _groupService.GetAllAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Committee")]
        public async Task<ActionResult<GroupResponse>> GetById([FromRoute] int id, CancellationToken ct)
        {
            var result = await _groupService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("my-group")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<GroupResponse>> GetMyGroup(CancellationToken ct)
        {
            var result = await _groupService.GetMyGroupAsync(ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<GroupResponse>> Create([FromBody] CreateGroupRequest request, CancellationToken ct)
        {
            var result = await _groupService.CreateAsync(request,ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpPost("{groupId}/members")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<GroupResponse>> AddMember([FromRoute]int groupId, [FromBody] AddMemberRequest request,CancellationToken ct)
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
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAvailableStudents(CancellationToken ct)
        {
            var result = await _groupService.GetAvailableStudentsAsync( ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
    }
}