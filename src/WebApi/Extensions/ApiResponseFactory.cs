using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

namespace WebApi.Extensions;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Payload { get; set; }
    public CustomProblemDetails? ErrorDetails { get; set; }
    public string? Message { get; set; } 
    public int HttpStatusCode { get; set; }
}

public sealed class CustomProblemDetails
{
    public string Type { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public int Status { get; init; }

    public string Detail { get; init; } = string.Empty;

    public string Instance { get; init; } = string.Empty;
    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>(StringComparer.Ordinal);
}

public static class ApiResponseFactory
{
    public static ApiResponse<T> SuccessResponse<T>(T data, string message = "OK", int statusCode = 200)
        => new() { IsSuccess = true, Payload = data, ErrorDetails = null, Message = message, HttpStatusCode = statusCode };

    public static ApiResponse<T> ErrorResponse<T>(CustomProblemDetails problem, string message, int statusCode)
        => new() { IsSuccess = false, Payload = default, ErrorDetails = problem, Message = message, HttpStatusCode = statusCode };
}