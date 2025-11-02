using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Factories;

namespace WebApi.Filters;

public class ModelBindingEnvelopeFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                ApiResult.ValidationError(context.HttpContext, context.ModelState)
            );
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
