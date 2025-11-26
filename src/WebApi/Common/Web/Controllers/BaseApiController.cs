using FluentValidation;
using FluentValidation.Results;
using WebApi.Common.Web.Factories;

namespace WebApi.Common.Web.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected async Task<IActionResult?> ValidateAsync<TInput>(
        TInput input,
        IValidator<TInput> validator,
        CancellationToken ct
    )
        where TInput : class
    {
        ValidationResult? validation = await validator.ValidateAsync(input, ct);
        return !validation.IsValid
            ? BadRequest(ApiResult.ValidationError(HttpContext, validation))
            : null;
    }

    protected abstract IActionResult ToErrorResponse(Result result);
}
