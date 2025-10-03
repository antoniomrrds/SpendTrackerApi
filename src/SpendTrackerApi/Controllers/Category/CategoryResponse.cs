namespace SpendTracker.Api.Controllers.Category;

public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}