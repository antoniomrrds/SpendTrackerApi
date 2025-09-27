using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Entities;

internal sealed class Category
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _name = value.Trim();
                return;
            }
            string nameErrorMessage = ValidationMessages.RequiredField.FormatInvariant(nameof(Name));
            throw new DomainException(nameErrorMessage);
        }
    }

    public Category(string name)
    {
        Name = name;
    }
}