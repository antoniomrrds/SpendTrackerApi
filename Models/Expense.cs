namespace SpendTrackApi.Models;

public class Expense
{
    public int Id  { get; set; }
    public string Name  { get; set; } = string.Empty;
    public string Description  { get; set; } = string.Empty;
    public decimal Value  { get; set; }
    public DateTime Date  { get; set; } = DateTime.Now;
    public int CategoryId  { get; set; }
    public Category? Category { get; set; }
}
