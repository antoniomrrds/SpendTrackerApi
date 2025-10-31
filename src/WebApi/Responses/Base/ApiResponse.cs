namespace WebApi.Responses.Base;

public abstract class ApiResponse : ApiResponseBase
{
    public string Message { get; protected set; } = string.Empty;
}
