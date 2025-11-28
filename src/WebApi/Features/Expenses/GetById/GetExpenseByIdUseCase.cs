using SharedKernel.Abstractions;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.GetById;

public interface IGetExpenseByIdUseCase : IUseCase<GetExpenseByIdInput, Task<Result<ExpenseDto?>>>;

public record GetExpenseByIdInput(Guid Id);

internal class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpenseReaderRepository _expenseReaderRepo;

    public GetExpenseByIdUseCase(IExpenseReaderRepository expenseReaderRepo)
    {
        _expenseReaderRepo = expenseReaderRepo;
    }

    public async Task<Result<ExpenseDto?>> Perform(
        GetExpenseByIdInput input,
        CancellationToken cancellationToken = default
    )
    {
        return await _expenseReaderRepo.GetByIdAsync(input.Id, cancellationToken);
    }
}
