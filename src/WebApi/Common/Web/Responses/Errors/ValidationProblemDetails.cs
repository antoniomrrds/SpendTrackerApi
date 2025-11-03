namespace WebApi.Common.Web.Responses.Errors;

public class ValidationCustomProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]> Errors { get; init; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);
}
