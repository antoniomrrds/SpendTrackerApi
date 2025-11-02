using Application.Categories.Add;
using Application.Categories.Common;
using FluentValidation;
using WebApi.Factories;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace WebApi.Controllers.Categories.Add;

public class CreateCategoryController : CategoriesBaseController
{
    private readonly ICreateCategoryUseCase _useCase;
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryController(
        ICreateCategoryUseCase useCase,
        IValidator<CreateCategoryCommand> validator
    )
    {
        _useCase = useCase;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
    {
        CreateCategoryCommand command = new(request.Name, request.Description);
        ValidationResult? validation = await _validator.ValidateAsync(command);
        if (!validation.IsValid)
        {
            return BadRequest(ApiResult.ValidationError(HttpContext, validation));
        }

        Result<CategoryDto> result = await _useCase.Perform(command);
        return result.IsSuccess
            ? Ok(ApiResult.SuccessWithMessage(result.Value, "Categoria criada com sucesso"))
            : Conflict(ApiResult.Conflict(HttpContext, result.Error.Description));
    }
}
