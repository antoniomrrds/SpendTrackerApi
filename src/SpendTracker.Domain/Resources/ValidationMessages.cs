
using System.Text;


namespace SpendTracker.Domain.Resources;

public static class ValidationMessages
{
    public static readonly CompositeFormat RequiredField = CompositeFormat.Parse("The field {0} is required.");
    public static readonly CompositeFormat InvalidValue = CompositeFormat.Parse("The provided value is invalid.");
    public static readonly CompositeFormat CategoryNotFound = CompositeFormat.Parse("Category not found.");
}