namespace SpendTracker.Api.Controllers.Expense;

internal record ExpenseResponse
{
    public int Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public string DateFormatted => Date.ToString("dd/MM/yyyy HH:mm:ss");
    public decimal Value { get; init; }
    public int CategoryId { get; init; }
    public string CategoryName { get; init; } = string.Empty;
    public string FormattedValue => Value.ToString("C");
}