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
        dto.DateFormatted = BaseMappers.FormatDate(dto.Date);
        dto.AmountFormatted = BaseMappers.FormatCurrency(dto.Amount);
        return dto;
    }

    public static ExpenseDto ToQueryDto(this Expense expense, Category category) =>
        MapToBaseDto(expense, category);

    public static ExpenseDto ToFormattedDto(this ExpenseDto queryDto) => ApplyFormatting(queryDto);

    public static ExpenseDto ToDto(this Expense expense, Category category) =>
        ApplyFormatting(MapToBaseDto(expense, category));
}
