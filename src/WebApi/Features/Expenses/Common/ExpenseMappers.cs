using SharedKernel.Common;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;

namespace WebApi.Features.Expenses.Common;

internal static class ExpenseMappers
{
    private static ExpenseDto MapToBaseDto(Expense expense, Category category)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.IdCategory,
            CategoryName = category.Name,
        };
    }

    private static ExpenseDto ApplyFormatting(ExpenseDto dto)
    {
        dto.DateFormatted = dto.Date.ToFormattedDate();
        dto.AmountFormatted = dto.Amount.ToFormattedCurrency();
        return dto;
    }

    public static ExpenseDto ToDto(this Expense expense, Category category) =>
        ApplyFormatting(MapToBaseDto(expense, category));
}
