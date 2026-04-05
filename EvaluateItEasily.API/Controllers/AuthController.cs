using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Auth;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
namespace EvaluateItEasily.API.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Login")]

        public async Task<ActionResult<AuthResponse>> LoginAsync([FromBody]Core.Auth.LoginRequest loginRequest,CancellationToken cancellationToken=default!)
        {
            var result = await _authService.LoginAsync(loginRequest, cancellationToken);
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();

        }


        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Core.Auth.RegisterRequest registerRequest, CancellationToken cancellationToken = default!)
        {
            var result = await _authService.RegisterAsync(registerRequest, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var result = await _authService.ConfirmEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgerPassword([FromBody] Core.DTO_s.Auth.ForgotPasswordRequest request)
        {
            var result = await _authService.SendResetPasswordCodeAsync(request.Email);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Core.DTO_s.Auth.ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
        }
        [HttpPost("resend-email")]
        public async Task<IActionResult> ResendEmail([FromBody] Core.DTO_s.Auth.ResendConfirmationEmailRequest request)
        {
            var result = await _authService.ResendConfirmationEmailAsync(request);

            return result.IsSuccess ? Ok() : result.ToProblem();
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
