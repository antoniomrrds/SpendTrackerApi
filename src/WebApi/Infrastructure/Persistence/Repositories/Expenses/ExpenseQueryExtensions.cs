using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Common;

namespace WebApi.Infrastructure.Persistence.Repositories.Expenses;

public static class ExpenseQueryExtensions
{
    public static IQueryable<ExpenseDto> SelectExpenseWithCategory(
        this IQueryable<Expense> expenses,
        IQueryable<Category> categories
    )
    {
        return expenses.Join(
            categories,
            expense => expense.IdCategory,
            category => category.Id,
            (expense, category) => expense.ToDto(category)
        );
    }
}
