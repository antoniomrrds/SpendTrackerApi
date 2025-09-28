using SpendTracker.Domain.Validation;

namespace SpendTracker.Domain.Entities;

internal sealed class Category
{
    public Guid Id { get; init; }

    private string _name = string.Empty;
    private string _description = string.Empty;

    public string Name
    {
        get => _name;
        set => _name = DomainValidation.RequiredAndTrim(value, nameof(Name));
    }

    public string Description
    {
        get => _description;
        set => _description = DomainValidation.MaxLength(value, nameof(Description), 200);
    }

    public Category(string name, string description = "")
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
    }

    internal Category() { }

}