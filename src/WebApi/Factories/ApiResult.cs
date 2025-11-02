using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Responses.Errors;
using WebApi.Responses.Success;

namespace WebApi.Factories;

public static class ApiResult
{
    public static ApiSuccessResponse<T> Success<T>(
        T data,
        string message = "Sucesso",
        int statusCode = 200
    ) => new(data, message, statusCode);

    public static ProblemDetails NotFound(
        HttpContext context,
        string errorMessage,
        string title = "Recurso não encontrado"
    ) => CreateProblem(context, StatusCodes.Status404NotFound, title, errorMessage);

    public static ProblemDetails Conflict(
        HttpContext context,
        string errorMessage,
        string title = "Conflito de dados"
    ) => CreateProblem(context, StatusCodes.Status409Conflict, title, errorMessage);

    public static ProblemDetails BadRequest(
        HttpContext context,
        string errorMessage,
        string title = "Requisição inválida"
    ) => CreateProblem(context, StatusCodes.Status400BadRequest, title, errorMessage);

    public static ValidationCustomProblemDetails ValidationError(
        HttpContext context,
        ValidationResult validationResult
    ) =>
        CustomValidationProblemDetailsFactory.ValidationFromFluentValidation(
            context,
            validationResult
        );

    public static ValidationCustomProblemDetails ValidationError(
        HttpContext context,
        ModelStateDictionary modelState
    ) => CustomValidationProblemDetailsFactory.ValidationFromModelState(context, modelState);

    private static ProblemDetails CreateProblem(
        HttpContext context,
        int statusCode,
        string title,
        string detail
    ) => CustomProblemDetailsFactory.Create(context, statusCode, title, detail);
}
