using FluentValidation;
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
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request,
        CancellationToken ct
    )
    {
        CreateCategoryInput input = new()
        {
            Name = request.Name,
            Description = request.Description,
        };

        if (await ValidateAsync(input, _validator, ct) is { } error)
            return error;

        Result<CategoryDto> result = await _useCase.Perform(input, ct);
        return result.IsSuccess
            ? Ok(ApiResult.SuccessWithMessage(result.Value, "Categoria criada com sucesso"))
            : ToErrorResponse(result);
    }
}

public record CreateCategoryRequest(string Name, string? Description);
