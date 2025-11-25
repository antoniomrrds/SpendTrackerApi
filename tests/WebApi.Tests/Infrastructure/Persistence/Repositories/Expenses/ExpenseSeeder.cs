using WebApi.Domain.Expenses;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

internal static class ExpenseSeeder
{
    public static async Task AddAsync(AppDbContext context, Expense expense)
    {
        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();
    }

    public static async Task AddRangeAsync(AppDbContext context, IEnumerable<Expense> expenses)
    {
        await context.Expenses.AddRangeAsync(expenses);
        await context.SaveChangesAsync();
    }
}
