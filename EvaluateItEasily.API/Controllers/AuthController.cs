using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Auth;
using EvaluateItEasily.Core.Contracts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]

        public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody]LoginRequest loginRequest,CancellationToken cancellationToken=default!)
        {
            var result = await _authService.LoginAsync(loginRequest, cancellationToken);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();

        }
        [HttpPost("Register")]

        public async Task<ActionResult<AuthResponse>> RegisterAsync([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken = default!)
        {
            var result = await _authService.RegisterAsync(registerRequest, cancellationToken);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();

        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeTokenAsync(request.RefreshToken, cancellationToken);
            return result.IsSuccess ? Ok() : result.ToProblem();
        }


    }
}
