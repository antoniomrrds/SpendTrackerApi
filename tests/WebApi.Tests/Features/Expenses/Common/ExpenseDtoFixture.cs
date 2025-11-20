using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Common;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Domain.Expenses;

namespace WebApi.Tests.Features.Expenses.Common;

internal static class ExpenseDtoFixture
{
    public static ExpenseDto GetExpenseDto(bool useNewSeed = false)
    {
        Expense expense = ExpenseFixture.GetExpense(useNewSeed);
        Category category = CategoryFixture.GetCategory(useNewSeed);
        return expense.ToDto(category);
    }
}
