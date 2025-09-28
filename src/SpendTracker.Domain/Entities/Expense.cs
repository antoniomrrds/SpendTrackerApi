using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;
using SpendTracker.Domain.Validation;

namespace SpendTracker.Domain.Entities;

public sealed class Expense
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public Guid CategoryId { get; private set; }

    public Expense(string description, decimal amount, DateTime date, Guid categoryId)
    {
        ValidateDate(date);
        Id = Guid.NewGuid();
        Description = ValidateDescription(description);
        Amount = ValidateAmount(amount);
        Date = date;
        CategoryId = categoryId;
    }

    internal Expense() { }

    public void SetAmount(decimal newAmount)
    {
        ValidateAmount(newAmount);
        Amount = newAmount;
    }

    private static decimal ValidateAmount(decimal amount)
    {
       return DomainValidation.GreaterThan(amount, nameof(Amount), 0);
    }

    public void SetDescription(string newDescription)
    {
        ValidateDescription(newDescription);
        Description = newDescription;
    }
    
    public void SetDate(DateTime newDate)
    {
        ValidateDate(newDate);
        Date = newDate;
    }
    
    private static void ValidateDate(DateTime date)
    {
        if (date > DateTime.Now)
        {
            throw new DomainException(ValidationMessages.DateIsFuture.FormatInvariant(nameof(Date)));
        }
    }

    private static string ValidateDescription(string description)
    {
     return  DomainValidation.RequiredAndTrim(description, nameof(Description));
    }
}
