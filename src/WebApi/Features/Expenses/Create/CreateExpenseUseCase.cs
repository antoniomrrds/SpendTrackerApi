using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Data;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Categories.Common;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.Create;

public interface ICreateExpenseUseCase : IUseCase<CreateExpenseInput, Task<Result<ExpenseDto>>>;

public record CreateExpenseInput
{
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public Guid IdCategory { get; init; }
}

internal class CreateExpenseUseCase : ICreateExpenseUseCase
{
    private readonly ICategoryCheckRepository _categoryCheckRepo;
    private readonly IExpenseWriterRepository _expenseWriterRepo;
    private readonly IExpenseReaderRepository _expenseReaderRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseUseCase(
        ICategoryCheckRepository categoryCheckRepo,
        IExpenseWriterRepository expenseWriterRepo,
        IUnitOfWork unitOfWork,
        IExpenseReaderRepository expenseReaderRepo
    )
    {
        _categoryCheckRepo = categoryCheckRepo;
        _expenseWriterRepo = expenseWriterRepo;
        _unitOfWork = unitOfWork;
        _expenseReaderRepo = expenseReaderRepo;
    }

    public async Task<Result<ExpenseDto>> Perform(
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

        bool categoryExists = await _categoryCheckRepo.CategoryExistsAsync(
            input.IdCategory,
            cancellationToken
        );
        if (!categoryExists)
        {
            return CategoryErrors.NotFound(input.IdCategory.ToString());
        }

        await _expenseWriterRepo.AddAsync(expense, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        ExpenseDto? created = await _expenseReaderRepo.GetByIdAsync(
            id: expense.Id,
            cancellationToken
        );
        return created;
    }
}
