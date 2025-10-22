using WebApi.Responses.Models;

namespace WebApi.Responses.Errors;

public sealed class ApiValidationErrorsResponse  : ApiErrorBase
{
    public IDictionary<string, string[]> Errors { get; init; } =
        new Dictionary<string, string[]>(StringComparer.Ordinal);
    
    public ApiValidationErrorsResponse (){}

    public ApiValidationErrorsResponse(CustomProblemDetails details)
    {
        Errors = details.Errors ;
        InitializeFromDetails(details);
    }
}