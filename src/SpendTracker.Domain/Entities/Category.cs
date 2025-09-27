using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Entities;

internal sealed class Category
{
    public Guid Id { get; init; }

    private string _name = string.Empty;
    private string _description = string.Empty;

    public string Name
    {
        get => _name;
        set => _name = ValidateRequiredAndTrim(value, nameof(Name));
    }

    public string Description
    {
        get => _description;
        set => _description = ValidateMax200Chars(value, nameof(Description));
    }

    public Category(string name, string description = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    private static string ValidateMax200Chars(string value, string fieldName = "")
    {
        const int MAXCHAR = 200;
        string valueWithoutSpace = value.Trim();
        if (valueWithoutSpace.Length == 0 || valueWithoutSpace.Length <= MAXCHAR && !string.IsNullOrWhiteSpace(value))
            return value.Trim();
        
        string message = ValidationMessages.MaxChars.FormatInvariant(fieldName, MAXCHAR);
        throw new DomainException(message);
    }

    private static string ValidateRequiredAndTrim(string value, string fieldName = "")
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value.Trim();
        }

        string message = ValidationMessages.RequiredField.FormatInvariant(fieldName);
        throw new DomainException(message);
    }
}