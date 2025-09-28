namespace SpendTracker.Domain.Entities;

internal sealed class Expense
{
    public Guid Id { get; init; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public Expense(string description, decimal value, DateTime date, Guid categoryId)
    {
        Id = Guid.NewGuid();
        Description = description;
        Value = value;
        Date = date;
        CategoryId = categoryId;
    }
    
    internal  Expense()
    { }
};