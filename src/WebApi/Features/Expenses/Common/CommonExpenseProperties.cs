namespace WebApi.Features.Expenses.Common;

public abstract record CommonExpenseProperties
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid IdCategory { get; init; }
}
