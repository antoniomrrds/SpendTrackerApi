using FluentValidation;
using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;
using WebApi.Common.Web.Filters;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Update;

public class UpdateCategoryController : CategoryBaseController
{
    private readonly IUpdateCategoryUseCase _useCase;
    private readonly IValidator<UpdateCategoryInput> _validator;

    public UpdateCategoryController(
        IUpdateCategoryUseCase useCase,
        IValidator<UpdateCategoryInput> validator
    )
    {
        _useCase = useCase;
        _validator = validator;
    }

    [HttpPut("{id}")]
    [ServiceFilter(typeof(ModelBindingEnvelopeFilter))]
    public async Task<IActionResult> Update(
        [FromRoute] SafeGuid id,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken ct
    )
    {
        UpdateCategoryInput input = new()
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
        };

        if (await ValidateAsync(input, _validator, ct) is { } error)
            return error;

        Result<CategoryDto> result = await _useCase.Perform(input, ct);
        return result.IsSuccess ? Ok(ApiResult.Success(result.Value)) : ToErrorResponse(result);
    }
}

public record UpdateCategoryRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
