namespace WebApi.Features.Categories.Common;

public abstract record CommonCategoryProperties
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
