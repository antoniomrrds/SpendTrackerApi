namespace WebApi.Common.Web.Responses.Success;

public abstract class ApiSuccessResponseBase<T>(T payload, int status)
{
    public int Status { get; init; } = status;

    public T? Payload { get; init; } = payload;
}

public sealed class ApiSuccessResponse<T>(T payload, int status = 200)
    : ApiSuccessResponseBase<T>(payload, status);

public sealed class ApiSuccessResponseWithMessage<T>(
    T payload,
    string message = "OK",
    int status = 200
) : ApiSuccessResponseBase<T>(payload, status)
{
    public string Message { get; init; } = message;
}
