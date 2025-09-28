using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Validation;

internal static class DomainValidation
{
    public static string RequiredAndTrim(string value, string fieldName)
    {
        return !string.IsNullOrWhiteSpace(value) 
            ? value.Trim() 
            : throw new DomainException(ValidationMessages.RequiredField.FormatInvariant(fieldName));
    }

    public static string MaxLength(string? value, string fieldName, int maxLength)
    {
        string trimmed = value?.Trim() ?? string.Empty;

        return string.IsNullOrWhiteSpace(trimmed) || trimmed.Length <= maxLength
            ? trimmed
            : throw new DomainException(ValidationMessages.MaxChars.FormatInvariant(fieldName, maxLength));
    }
    
    public static decimal GreaterThan(decimal number, string fieldName, decimal minValue)
    {
        return number > minValue
            ? number
            : throw new DomainException(ValidationMessages.GreaterThan.FormatInvariant(fieldName, minValue));
    }

    public static DateTime DateIsFuture(DateTime expectedDate)
    {
        return DateTime.Now;
    }
}