using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Domain.Expenses;

namespace WebApi.Tests.Features.Expenses.Create;

public static class CreateExpenseFixture
{
    public static CreateExpenseInput ExpenseInput(bool useNewSeed = false)
    {
        Expense expense = ExpenseFixture.GetExpense(useNewSeed);
        return new CreateExpenseInput()
        {
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            IdCategory = expense.IdCategory,
        };
    }
}
