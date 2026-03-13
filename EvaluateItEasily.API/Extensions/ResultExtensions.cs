using EvaluateItEasily.Core.DTO_s;
using EvaluateItEasily.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace EvaluateItEasily.API.Extensions
{
    public static class ResultExtensions
    {
        public static ObjectResult ToProblem(this Result result)
        {
            if (result.IsSuccess)
                throw new InvalidOperationException("Can't convert success result to a problem");

            var problem = Results.Problem(statusCode: result.Error.StatusCode);
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>
            {
                {
                    "errors", new[] { result.Error.code,result.Error.ErrorDescription }
                }
            };

            return new ObjectResult(problemDetails);
        }
        public static IActionResult ToFileResult(this Result<FileDownloadResponse> result)
        {
            if (result.IsFailure)
                return result.Error.StatusCode switch
                {
                    StatusCodes.Status404NotFound => new NotFoundObjectResult(result.Error),
                    StatusCodes.Status401Unauthorized => new UnauthorizedObjectResult(result.Error),
                    _ => new BadRequestObjectResult(result.Error)
                };

            return new FileContentResult(result.Data.FileBytes, result.Data.ContentType)
            {
                FileDownloadName = result.Data.FileName
            };
        }
    }
}