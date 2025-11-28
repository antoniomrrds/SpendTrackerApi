using SharedKernel.Abstractions;
using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.GetById;

public interface IGetExpenseByIdUseCase : IUseCase<GetExpenseByIdInput, Task<Result<ExpenseDto?>>>;

public record GetExpenseByIdInput(Guid Id);

internal class GetExpenseByIdUseCase(IExpenseReaderRepository repo) : IGetExpenseByIdUseCase
{
    public async Task<Result<ExpenseDto?>> Perform(
        GetExpenseByIdInput input,
        CancellationToken cancellationToken = default
    )
    {
        ExpenseDto? result = await repo.GetByIdAsync(input.Id, cancellationToken);
        return result is null ? ExpenseErrors.NotFound(id: input.Id) : result;
    }
}
