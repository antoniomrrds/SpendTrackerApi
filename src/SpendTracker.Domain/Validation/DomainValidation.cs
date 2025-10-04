using SharedKernel.Resources;
using SpendTracker.Domain.Errors;

namespace SpendTracker.Domain.Validation;

internal static class DomainValidation
{
    public static string RequiredAndTrim(string value, string fieldName)
    {
        return !string.IsNullOrWhiteSpace(value)
            ? value.Trim()
            : throw new DomainException(
                ValidationMessageProvider.Get(ValidationKeys.RequiredField, FieldNameProvider.Get(fieldName)),
                FieldNameProvider.Get(fieldName));
    }

    public static string MaxLength(string? value, string fieldName, int maxLength)
    {
        var trimmed = value?.Trim() ?? string.Empty;

        return string.IsNullOrWhiteSpace(trimmed) || trimmed.Length <= maxLength
            ? trimmed
            : throw new DomainException(
                ValidationMessageProvider.Get(ValidationKeys.MaxChars, FieldNameProvider.Get(fieldName), maxLength),
                FieldNameProvider.Get(fieldName));
    }

    public static decimal GreaterThan(decimal number, string fieldName, decimal minValue)
    {
        return number > minValue
            ? number
            : throw new DomainException(
                ValidationMessageProvider.Get(ValidationKeys.GreaterThan, FieldNameProvider.Get(fieldName), minValue),
                FieldNameProvider.Get(fieldName));
    }

    public static DateTime DateIsFuture(DateTime expectedDate)
    {
        return expectedDate > DateTime.Today
            ? throw new DomainException(
                ValidationMessageProvider.Get(ValidationKeys.DateIsFuture, expectedDate.ToShortDateString()),
                "Date")
            : expectedDate;
    }
}