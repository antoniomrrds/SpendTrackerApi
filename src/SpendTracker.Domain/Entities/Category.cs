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
        set => _name = value.Trim();
    }

    public Category(string name)
    {

        if (string.IsNullOrWhiteSpace(name)) 
        {
            string nameErrorMessage = ValidationMessages.RequiredField.FormatInvariant(nameof(Name));
            throw new DomainException(nameErrorMessage);
        }

        Name = name;
        
    }
    
}
