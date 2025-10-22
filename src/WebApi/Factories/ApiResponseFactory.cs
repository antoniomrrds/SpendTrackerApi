using WebApi.Responses.Base;
using WebApi.Responses.Errors;
using WebApi.Responses.Models;
using WebApi.Responses.Success;

namespace WebApi.Factories;

public static class ApiResponseFactory
{
    public static ApiSuccessResponse<T> Success<T>(T data, string message = "OK", int statusCode = 200)
        => new(data, message, statusCode);

    private static ApiResponseBase FromProblemDetails(CustomProblemDetails details)
    {
        return details.HasValidationErrors 
            ? new ApiValidationErrorsResponse(details) 
            : new ApiErrorResponse(details);
    }
    
    public static ApiErrorResponse Error(CustomProblemDetails details)
        => (ApiErrorResponse)FromProblemDetails(details);

    public static ApiValidationErrorsResponse Errors(CustomProblemDetails details)
        => (ApiValidationErrorsResponse)FromProblemDetails(details);
}