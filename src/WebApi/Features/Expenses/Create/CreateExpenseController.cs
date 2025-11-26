using FluentValidation;
using WebApi.Common.Web.Factories;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.Create;

public class CreateExpenseController : ExpenseBaseController
{
    private readonly ICreateExpenseUseCase _useCase;
    private readonly IValidator<CreateExpenseInput> _validator;

    public CreateExpenseController(
        ICreateExpenseUseCase useCase,
        IValidator<CreateExpenseInput> validator
    )
    {
        _useCase = useCase;
        _validator = validator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateExpenseRequest request,
        CancellationToken ct
    )
    {
        CreateExpenseInput input = new()
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            IdCategory = request.IdCategory,
        };

        if (await ValidateAsync(input, _validator, ct) is { } error)
            return error;

        Result<ExpenseDto> result = await _useCase.Perform(input, ct);
        return result.IsSuccess ? Ok(ApiResult.Success(result.Value)) : ToErrorResponse(result);
    }
}

public record CreateExpenseRequest
{
    public required string Description { get; init; }
    public required decimal Amount { get; init; }
    public DateTime? Date { get; init; }
    public required Guid IdCategory { get; init; }
}
