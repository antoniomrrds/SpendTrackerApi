using SharedKernel.Common;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;

namespace WebApi.Features.Expenses.Common;

internal static class ExpenseMappers
{
    public static ExpenseDto ToDto(this Expense expense, Category category)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.IdCategory,
            CategoryName = category.Name,
            DateFormatted = BaseMappers.FormatDate(expense.Date),
            AmountFormatted = BaseMappers.FormatCurrency(expense.Amount),
        };
    }
}
