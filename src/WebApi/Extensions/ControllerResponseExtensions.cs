using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ControllerResponseExtensions
{
    public static IActionResult ToValidationProblem(this ControllerBase controller, ValidationResult validationResult)
    {
        IDictionary<string, string[]>? errors = validationResult.ToDictionary();
        CustomProblemDetails problem = new()
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "A solicitação contém erros de validação.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Um ou mais campos estão inválidos.",
            Instance = controller.HttpContext.Request.Path,
            Errors = errors
        };

        ApiResponse<CustomProblemDetails> response = ApiResponseFactory
            .ErrorResponse<CustomProblemDetails>(problem, "Erro de validação", problem.Status);
        return controller.BadRequest(response);
    }

    public static IActionResult ToNotFoundProblem(this ControllerBase controller, string detail, string message = "Recurso não encontrado")
    {
        CustomProblemDetails problem = new()
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.4",
            Title = "Recurso não encontrado.",
            Status = StatusCodes.Status404NotFound,
            Detail = detail,
            Instance = controller.HttpContext.Request.Path
        };

        ApiResponse<CustomProblemDetails> response = ApiResponseFactory.ErrorResponse<CustomProblemDetails>(problem, message, problem.Status);
        return controller.NotFound(response);
    }

    public static IActionResult ToConflictProblem(this ControllerBase controller, string detail, string message = "Conflito de dados")
    {
        CustomProblemDetails problem = new()
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            Title = "Conflito de dados.",
            Status = StatusCodes.Status409Conflict,
            Detail = detail,
            Instance = controller.HttpContext.Request.Path
        };

        ApiResponse<CustomProblemDetails> response = ApiResponseFactory.ErrorResponse<CustomProblemDetails>(problem, message, problem.Status);
        return controller.Conflict(response);
    }

    public static IActionResult ToOkResponse<T>(
        this ControllerBase controller,
        T data, string message = "Sucesso",
        int statusCode = StatusCodes.Status200OK
        )
    {
        ApiResponse<T> response = ApiResponseFactory.SuccessResponse(data, message, statusCode);
        return controller.StatusCode(statusCode, response);
    }
}
