using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Common.Web.Extensions;
using WebApi.Common.Web.Helper;
using WebApi.Common.Web.Responses.Errors;

namespace WebApi.Common.Web.Factories;

internal static class CustomValidationProblemDetailsFactory
{
    public static ValidationCustomProblemDetails ValidationFromFluentValidation(
        HttpContext context,
        ValidationResult validationResult,
        string? instance = null,
        string? title = "A solicitação contém erros de validação.",
        string? type = null
    )
    {
        IDictionary<string, string[]>? errors = validationResult.ToDictionary();
        const int statusCode = StatusCodes.Status400BadRequest;
        ValidationCustomProblemDetails problem = new()
        {
            Status = statusCode,
            Title = title,
            Detail = "Um ou mais campos estão inválidos.",
            Instance = instance ?? context.GetRequestSignature(),
            Type = type ?? ProblemDetailsTypeHelper.GetDefaultType(statusCode),
            Errors = errors,
        };

        return problem;
    }

    public static ValidationCustomProblemDetails ValidationFromModelState(
        HttpContext context,
        ModelStateDictionary modelState,
        string? instance = null,
        string? title = "A solicitação contém erros de binding.",
        string? type = null
    )
    {
        Dictionary<string, string[]> errors = modelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );

        const int statusCode = StatusCodes.Status400BadRequest;

        return new ValidationCustomProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = "Um ou mais parâmetros estão inválidos.",
            Instance = instance ?? context.GetRequestSignature(),
            Type = type ?? ProblemDetailsTypeHelper.GetDefaultType(statusCode),
            Errors = errors,
        };
    }
}
