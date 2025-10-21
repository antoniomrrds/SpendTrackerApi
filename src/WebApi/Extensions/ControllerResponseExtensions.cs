using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApi.Factories;
using WebApi.Responses.Errors;
using WebApi.Responses.Models;
using WebApi.Responses.Success;

namespace WebApi.Extensions;

public static class ControllerResponseExtensions
{
    public static IActionResult ToValidationProblem(this ControllerBase controller, ValidationResult validationResult)
    {
        IDictionary<string, string[]>? errors = validationResult.ToDictionary();
        CustomProblemDetails problem = new()
        {
            Title = "A solicitação contém erros de validação.",      
            Status = StatusCodes.Status400BadRequest,
            Instance = controller.HttpContext.Request.Path,
            Errors = errors
        };

        ApiValidationErrorsResponse  response = ApiResponseFactory.Errors(problem);

        return controller.BadRequest(response);
    }

    public static IActionResult ToNotFoundProblem(this ControllerBase controller, string errorMessage,
        string message = "Recurso não encontrado")
    {
        CustomProblemDetails problem = new()
        {
            Title = message,
            Status = StatusCodes.Status404NotFound,
            Error = errorMessage,
            Instance = controller.HttpContext.Request.Path
        };

        ApiErrorResponse response = ApiResponseFactory.Error(problem);
        return controller.NotFound(response);
    }

    public static IActionResult ToConflictProblem(this ControllerBase controller, string errorMessage,
        string message = "Conflito de dados")
    {
        CustomProblemDetails problem = new()
        {
            Title = message,
            Status = StatusCodes.Status409Conflict,
            Error = errorMessage,
            Instance = controller.HttpContext.Request.Path
        };

        ApiErrorResponse response = ApiResponseFactory.Error(problem);
        return controller.Conflict(response);
    }

    public static IActionResult ToOkResponse<T>(
        this ControllerBase controller,
        T data, string message = "Sucesso",
        int statusCode = StatusCodes.Status200OK
    )
    {
        ApiSuccessResponse<T> response = ApiResponseFactory.Success(data, message, statusCode);
        return controller.StatusCode(statusCode, response);
    }
}