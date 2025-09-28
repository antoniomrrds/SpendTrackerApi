namespace SpendTracker.Domain.Entities;

internal sealed class Expense
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Value { get; private set; }
    public DateTime Date { get; private set; }
    public Guid CategoryId { get; private set; }
    public Expense(string description, decimal value, DateTime date, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Description = description;
        Value = value;
        Date = date;
        CategoryId = categoryId;
    }
    
    internal Expense()
    { }
};