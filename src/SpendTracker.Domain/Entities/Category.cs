using SpendTracker.Domain.Validation;

namespace SpendTracker.Domain.Entities;

public sealed class Category
{
    public Guid Id { get; init; }

    public string Name { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;
    
    //ef constructor
    private Category() { }

    public Category(string name, string description = "")
    {
        Id = Guid.NewGuid();
        Name = ValidateName(name);
        Description = ValidateDescription(description);
    }

    public void SetName(string name)
    {
        Name = ValidateName(name);
    }
    
    public void SetDescription(string description)
    {
        Description = ValidateDescription(description);
    }

    private static string ValidateDescription(string description) =>
        DomainValidation.MaxLength(description, nameof(Description), 200);

    private static string ValidateName(string name) => DomainValidation.RequiredAndTrim(name, nameof(Name));
}