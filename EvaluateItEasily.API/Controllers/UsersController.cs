using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll([FromQuery] string? role,CancellationToken ct)
        {
            var result = await _userService.GetAllAsync(role, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetById(string id,CancellationToken ct)
        {
            var result = await _userService.GetByIdAsync(id, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create([FromBody] CreateUserRequest request,CancellationToken ct)
        {
            var result = await _userService.CreateAsync(request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponse>> Update(string id,[FromBody] UpdateUserRequest request,CancellationToken ct)
        {
            var result = await _userService.UpdateAsync(id, request, ct);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }

        [HttpPut("{id}/toggle-active")]
        public async Task<ActionResult<UserResponse>> ToggleActive(string id,CancellationToken ct)
        {
            var result = await _userService.ToggleActiveAsync(id, ct);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }
    }
}
