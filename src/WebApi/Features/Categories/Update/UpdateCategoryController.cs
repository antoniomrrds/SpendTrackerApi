using FluentValidation;
using FluentValidation.Results;
using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;
using WebApi.Common.Web.Filters;
using WebApi.Domain.Categories;
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
        [FromBody] UpdateCategoryRequest request
    )
    {
        UpdateCategoryInput input = new()
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
        };

        ValidationResult? validation = await _validator.ValidateAsync(input);
        if (!validation.IsValid)
        {
            return BadRequest(ApiResult.ValidationError(HttpContext, validation));
        }

        Result<CategoryDto> result = await _useCase.Perform(input);
        return result.IsSuccess
            ? Ok(ApiResult.Success(result.Value))
            : result.Error.Code switch
            {
                CategoryErrorCodes.NameAlreadyExists => Conflict(
                    ApiResult.Conflict(HttpContext, result.Error.Description)
                ),
                _ => NotFound(ApiResult.NotFound(HttpContext, result.Error.Description)),
            };
    }
}

public record UpdateCategoryRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
