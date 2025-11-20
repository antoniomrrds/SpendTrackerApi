namespace WebApi.Domain.Expenses;

internal static class ExpenseErrors
{
    public static FailureReason NotFound(Guid id) =>
        new(ExpenseErrorCodes.NotFound, $"A despesa com o id: '{id}' não foi encontrada.");
}
