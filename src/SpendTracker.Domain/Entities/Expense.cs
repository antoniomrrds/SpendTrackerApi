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
        ValidateAmount(amount);
        DomainValidation.RequiredAndTrim(description, nameof(Description));

        Id = Guid.NewGuid();
        Description = description;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
    }

    internal Expense() { }

    public void SetAmount(decimal newAmount)
    {
        ValidateAmount(newAmount);
        Amount = newAmount;
    }

    private static void ValidateAmount(decimal amount)
    {
        DomainValidation.GreaterThan(amount, nameof(Amount), 0);
    }

    public void SetDescription(string newDescription)
    {
        DomainValidation.RequiredAndTrim(newDescription, nameof(Description));
        Description = newDescription;
    }
}
