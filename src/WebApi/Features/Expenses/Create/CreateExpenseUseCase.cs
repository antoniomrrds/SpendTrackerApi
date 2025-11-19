using System.Globalization;
using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Categories.Common;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.Create;

public interface ICreateExpenseUseCase : IUseCase<CreateExpenseInput, Task<Result<bool>>>;

public record CreateExpenseInput
{
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public Guid IdCategory { get; init; }
}

internal record ExpenseResponse
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string DateFormatted =>
        Date.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    public decimal Amount { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string FormattedValue => Amount.ToString("C", CultureInfo.InvariantCulture);
}

internal class CreateExpenseUseCase : ICreateExpenseUseCase
{
    private readonly ICategoryCheckRepository _categoryCheckRepo;
    private readonly IExpenseWriterRepository _expenseWriterRepo;

    public CreateExpenseUseCase(
        ICategoryCheckRepository categoryCheckRepo,
        IExpenseWriterRepository expenseWriterRepo
    )
    {
        _categoryCheckRepo = categoryCheckRepo;
        _expenseWriterRepo = expenseWriterRepo;
    }

    public async Task<Result<bool>> Perform(
        CreateExpenseInput input,
        CancellationToken cancellationToken = default
    )
    {
        Expense expense = new(
            id: Guid.NewGuid(),
            description: input.Description,
            amount: input.Amount,
            date: input.Date,
            idCategory: input.IdCategory
        );

        bool categoryExists = await _categoryCheckRepo.CategoryExists(
            input.IdCategory,
            cancellationToken
        );
        if (!categoryExists)
        {
            return CategoryErrors.NotFound(input.IdCategory.ToString());
        }

        await _expenseWriterRepo.AddAsync(expense, cancellationToken);

        return true;
    }
}
