namespace WebApi.Responses.Models;

public sealed record CustomProblemDetails
{
    public string Title { get; init; } = string.Empty;
    public string Error { get; init; } = string.Empty;
    public int Status { get; init; } = 500;
    public string Instance { get; init; } = string.Empty;
    public IDictionary<string, string[]> Errors { get; init; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);
    public bool HasValidationErrors => Errors is { Count: > 0 };
}
