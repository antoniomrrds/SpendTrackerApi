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

    public static CreateExpenseRequest ValidRequest(bool useNewSeed = false)
    {
        Expense expense = ExpenseFixture.GetExpense(useNewSeed);
        return new CreateExpenseRequest()
        {
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            IdCategory = expense.IdCategory,
        };
    }

    public static CreateExpenseRequest InvalidRequest()
    {
        DateTime futureDate = DateTime.Today.AddDays(1);

        return new CreateExpenseRequest()
        {
            Description = string.Empty,
            Amount = 0,
            Date = futureDate,
            IdCategory = Guid.Empty,
        };
    }
}
