using Domain.Validation;

namespace Domain.Expenses;

public sealed class Expense
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public Guid IdCategory { get; private set; }

    public Expense(string description, decimal amount, DateTime date, Guid idCategory)
    {
        Id = Guid.NewGuid();
        Description = ValidateDescription(description);
        Amount = ValidateAmount(amount);
        Date = ValidateDate(date);
        IdCategory = idCategory;
    }
    //ef constructor
    private Expense() { }

    public void SetAmount(decimal newAmount)
    {
        Amount = ValidateAmount(newAmount);
    }

    public void SetDescription(string newDescription)
    {
        Description = ValidateDescription(newDescription);
    }

    public void SetDate(DateTime newDate)
    {
        Date = ValidateDate(newDate);
    }

    private static DateTime ValidateDate(DateTime date)
    {
        return DomainValidation.DateIsFuture(date);
    }

    private static string ValidateDescription(string description)
    {
        return DomainValidation.RequiredAndTrim(description, nameof(Description));
    }

    private static decimal ValidateAmount(decimal amount)
    {
        return DomainValidation.GreaterThan(amount, nameof(Amount), 0);
    }
}