using FluentValidation;
using FluentValidation.Results;
using WebApi.Common.Web.Factories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Create;

public class CreateCategoryController : CategoryBaseController
{
    private readonly ICreateCategoryUseCase _useCase;
    private readonly IValidator<CreateCategoryInput> _validator;

    public CreateCategoryController(
        ICreateCategoryUseCase useCase,
        IValidator<CreateCategoryInput> validator
    )
    {
        _useCase = useCase;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        CreateCategoryInput input = new()
        {
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
            ? Ok(ApiResult.SuccessWithMessage(result.Value, "Categoria criada com sucesso"))
            : Conflict(ApiResult.Conflict(HttpContext, result.Error.Description));
    }
}

public record CreateCategoryRequest(string Name, string? Description);
