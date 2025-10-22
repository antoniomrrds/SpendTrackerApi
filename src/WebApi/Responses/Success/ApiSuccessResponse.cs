using WebApi.Responses.Base;

namespace WebApi.Responses.Success;

public sealed class ApiSuccessResponse<T> : ApiResponse
{
    public T? Payload { get; init; }

    public ApiSuccessResponse(T payload, string message = "OK", int statusCode = 200)
    {
        IsSuccess = true;
        Payload = payload;
        Message = message;
        StatusCode = statusCode;
    }
}