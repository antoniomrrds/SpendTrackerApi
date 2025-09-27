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
        set => _name = ValidateRequiredAndTrim(value , nameof(Name));
    }
    
    public Category(string name)
    {
        Name = name;
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