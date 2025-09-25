namespace SpendTracker.Domain.Entities;

internal sealed class Category
{
    public string Name { get; set; }

    public Category(string name)
    {
        Name = name;
    }
    
}
