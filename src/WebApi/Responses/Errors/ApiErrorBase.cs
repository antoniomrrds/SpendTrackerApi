using WebApi.Responses.Base;
using WebApi.Responses.Models;

namespace WebApi.Responses.Errors;

public abstract class ApiErrorBase : ApiResponseBase
{
    public string Title { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;

    protected void InitializeFromDetails(CustomProblemDetails details)
    {
        IsSuccess = false;
        Title = details.Title;
        Instance = details.Instance;
        StatusCode = details.Status;
    }
}
