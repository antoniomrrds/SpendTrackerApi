namespace SpendTrackerApi.Models;

public sealed class ExpenseEntity
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
}