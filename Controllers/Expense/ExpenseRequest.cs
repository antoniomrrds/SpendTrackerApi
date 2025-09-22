namespace SpendTrackApi.Controllers.Expense;

public record ExpenseRequest(
    decimal Value,
    string Description,
    int CategoryId,
    DateTime? Date);
