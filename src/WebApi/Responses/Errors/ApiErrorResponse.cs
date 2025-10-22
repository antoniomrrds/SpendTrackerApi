using WebApi.Responses.Models;

namespace WebApi.Responses.Errors;

public sealed class ApiErrorResponse : ApiErrorBase
{
    public string Error { get; init; } = string.Empty;
    public ApiErrorResponse() { }
    public ApiErrorResponse(CustomProblemDetails details)
    {
        Error = details.Error;
        InitializeFromDetails(details);
    }
}