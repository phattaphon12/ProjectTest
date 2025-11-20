using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Authentication.Helpers
{
    public class ApiResult
    {
        public static IActionResult Success<T>(T data, string message = "Success") where T : class
        {
            return new OkObjectResult(new
            {
                success = true,
                message = message,
                data = data
            });
        }

        public static IActionResult Fail(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)statusCode,
                Title = statusCode.ToString(),
                Detail = errorMessage
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)statusCode,
                ContentTypes = { "application/json" }
            };
        }
    }
}
