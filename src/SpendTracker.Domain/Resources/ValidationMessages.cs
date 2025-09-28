using System.Text;

namespace SpendTracker.Domain.Resources;

public static class ValidationMessages
{
    public static readonly CompositeFormat RequiredField =
        CompositeFormat.Parse("The field {0} is required.");

    public static readonly CompositeFormat InvalidValue =
        CompositeFormat.Parse("The value provided for {0} is invalid.");
        
    public static readonly CompositeFormat GreaterThan =
        CompositeFormat.Parse("The field {0} must be greater than {1}.");
    
    public static readonly CompositeFormat MaxChars =
        CompositeFormat.Parse("The field {0} cannot exceed {1} characters.");
}