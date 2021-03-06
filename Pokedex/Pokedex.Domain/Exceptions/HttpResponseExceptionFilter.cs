using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Pokedex.Domain.Exceptions
{
	public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
	{
		public int Order { get; set; } = int.MaxValue - 10;

		public void OnActionExecuting(ActionExecutingContext context)
		{
			//TODO add code to filter out exceptions on executing
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			if (context.Exception is HttpResponseException exception)
			{
				context.Result = new ObjectResult(exception.Value)
				{
					StatusCode = exception.Status,
				};
				context.ExceptionHandled = true;
			}
		}
	}
}
