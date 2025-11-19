using WebApi.Domain.Expenses;

namespace WebApi.Features.Expenses.Common;

public interface IExpenseWriterRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken = default);
}
