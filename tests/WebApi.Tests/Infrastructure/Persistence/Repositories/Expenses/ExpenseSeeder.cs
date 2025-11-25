using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

internal static class ExpenseSeeder
{
    public static async Task AddAsync(AppDbContext context, Expense expense, Category category)
    {
        await CategorySeeder.AddAsync(context, category);
        expense.IdCategory = category.Id;
        await context.Expenses.AddAsync(expense);
        await context.SaveChangesAsync();
    }

    public static async Task AddRangeAsync(AppDbContext context, IEnumerable<Expense> expenses)
    {
        await context.Expenses.AddRangeAsync(expenses);
        await context.SaveChangesAsync();
    }
}
