using Microsoft.EntityFrameworkCore;
using WebApi.Features.Expenses.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories.Expenses;

internal class ExpenseReaderRepository(AppDbContext context)
    : BaseRepository(context),
        IExpenseReaderRepository
{
    public async Task<ExpenseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        ExpenseDto? queryDto = await Context
            .Expenses.AsNoTracking()
            .Where(e => e.Id == id)
            .SelectExpenseWithCategory(Context.Categories)
            .FirstOrDefaultAsync(cancellationToken);
        return queryDto?.ToFormattedDto();
    }
}
