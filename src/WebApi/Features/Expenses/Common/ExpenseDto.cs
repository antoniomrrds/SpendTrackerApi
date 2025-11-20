namespace WebApi.Features.Expenses.Common;

public record ExpenseDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string DateFormatted { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string AmountFormatted { get; init; } = string.Empty;
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
}
