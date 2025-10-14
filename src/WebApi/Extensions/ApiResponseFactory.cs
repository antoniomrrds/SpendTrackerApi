using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Payload { get; set; }
    public ProblemDetails? ErrorDetails { get; set; }
    public string? Message { get; set; }
    public int HttpStatusCode { get; set; }
}


public static class ApiResponseFactory
{
    public static ApiResponse<T> SuccessResponse<T>(T data, string message = "OK", int statusCode = 200)
        => new() { IsSuccess = true, Payload = data, ErrorDetails = null, Message = message, HttpStatusCode = statusCode };

    public static ApiResponse<T> ErrorResponse<T>(ProblemDetails problem, string message, int statusCode)
        => new() { IsSuccess = false, Payload = default, ErrorDetails = problem, Message = message, HttpStatusCode = statusCode };
}