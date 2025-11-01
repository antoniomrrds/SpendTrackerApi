using Application.Categories.Add;
using Application.Categories.Common;
using FluentValidation;
using FluentValidation.Results;
using WebApi.Extensions;

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
            return this.ToValidationProblem(validation);

        Result<CategoryDto> result = await _useCase.Perform(command);
        return result.IsFailure
            ? this.ToConflictProblem(result.Error.Description, "Erro ao criar categoria")
            : this.ToOkResponse(result.Value, "Categoria criada com sucesso");
    }
}
