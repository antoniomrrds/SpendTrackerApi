using WebApi.Responses.Success;

namespace WebApi.Factories;

public static class ApiResponseFactory
{
    public static ApiSuccessResponse<T> Success<T>(
        T data,
        string message = "OK",
        int statusCode = 200
    ) => new(data, message, statusCode);
}
