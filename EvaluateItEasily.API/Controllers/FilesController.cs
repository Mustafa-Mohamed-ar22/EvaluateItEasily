using EvaluateItEasily.API.Extensions;
using EvaluateItEasily.Core.Contracts.Services;
using EvaluateItEasily.Core.DTO_s.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("presigned-upload")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<PresignedUploadResponse>> GetUploadUrl([FromQuery] string fileName,CancellationToken ct)
        {
            var result = (await _fileService.GenerateUploadUrlAsync(fileName, ct));
            return result.IsSuccess ? Ok(result.Data) : result.ToProblem();
        }
    }
}
