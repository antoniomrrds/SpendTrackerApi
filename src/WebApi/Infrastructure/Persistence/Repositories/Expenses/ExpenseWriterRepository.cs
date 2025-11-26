using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories.Expenses;

public class ExpenseWriterRepository(AppDbContext context)
    : BaseRepository(context),
        IExpenseWriterRepository
{
    public async Task AddAsync(Expense expense, CancellationToken cancellationToken = default)
    {
        await Context.Expenses.AddAsync(expense, cancellationToken);
    }
}
