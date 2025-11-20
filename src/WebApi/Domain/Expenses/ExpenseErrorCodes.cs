using WebApi.Domain.Errors;

namespace WebApi.Domain.Expenses;

internal static class ExpenseErrorCodes
{
    private const string Prefix = "CATEGORY";
    public const string NotFound = $"{Prefix}.{ErrorPatterns.NotFound}";
}
