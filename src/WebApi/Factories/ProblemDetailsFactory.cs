using FluentValidation.Results;
using WebApi.Constants;
using WebApi.Responses.Errors;

namespace WebApi.Factories;

public static class ProblemDetailsFactory
{
    public static ProblemDetails Create(
        int statusCode,
        string title,
        string detail,
        string instance,
        string? type = null!
    )
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = instance,
            Type = type ?? GetDefaultType(statusCode),
        };
    }

    public static ValidationCustomProblemDetails CreateValidation(
        ValidationResult validationResult,
        string instance,
        string title = "A solicitação contém erros de validação.",
        string? type = null!
    )
    {
        IDictionary<string, string[]>? errors = validationResult.ToDictionary();
        const int statusCode = StatusCodes.Status400BadRequest;
        ValidationCustomProblemDetails problem = new()
        {
            Status = statusCode,
            Title = title,
            Detail = "Um ou mais campos estão inválidos.",
            Instance = instance,
            Type = type ?? GetDefaultType(statusCode),
            Errors = errors,
        };

        return problem;
    }

    private static string GetDefaultType(int statusCode) =>
        statusCode switch
        {
            400 => ProblemDetailsTypes.BadRequest,
            404 => ProblemDetailsTypes.NotFound,
            409 => ProblemDetailsTypes.Conflict,
            _ => ProblemDetailsTypes.InternalServerError,
        };
}
