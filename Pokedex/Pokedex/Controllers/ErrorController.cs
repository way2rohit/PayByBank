using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
namespace Pokedex.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Handle Error in Local Development Enviorment
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (context != null)
            {
                //_logger.LogError(Infrastructure.Logger.Enums.ErrorCode.NotApplicable, $"{context.Error.Message} {context.Error.StackTrace}");
                return Problem(detail: context.Error.StackTrace, title: context.Error.Message);
            }
            else
            {
                //_logger.LogError(Infrastructure.Logger.Enums.ErrorCode.NotApplicable, $"detail: Unable to resolve IExceptionHandlerFeature in ErrorController title: Context was null");
                return Problem(detail: "Unable to resolve IExceptionHandlerFeature in ErrorController", title: "Context was null");
            }

        }

        /// <summary>
        /// Handles errors 
        /// </summary>
        /// <returns></returns>   
        [HttpGet()]
        public IActionResult Error() => Problem();
    }
}
