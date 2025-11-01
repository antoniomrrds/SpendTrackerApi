namespace WebApi.Responses.Success;

public sealed class ApiSuccessResponse<T>(T payload, string message = "OK", int status = 200)
{
    public string Message { get; init; } = message;
    public int Status { get; init; } = status;
    public T? Payload { get; init; } = payload;
}
