using EvaluateItEasily.Core.Contracts.Services;
using Microsoft.AspNetCore.Http;

using System.Security.Claims;

namespace EvaluateItEasily.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()=>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
    }
}
