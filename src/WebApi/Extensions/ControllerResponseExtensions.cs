using FluentValidation.Results;
using WebApi.Factories;
using WebApi.Responses.Errors;
using WebApi.Responses.Success;

namespace WebApi.Extensions;

public static class ControllerResponseExtensions
{
    public static IActionResult ToValidationProblem(
        this ControllerBase controller,
        ValidationResult validationResult
    )
    {
        ValidationCustomProblemDetails problemDetails = ProblemDetailsFactory.CreateValidation(
            validationResult,
            controller.HttpContext.Request.Path
        );

        return controller.BadRequest(problemDetails);
    }

    public static IActionResult ToNotFoundProblem(
        this ControllerBase controller,
        string errorMessage,
        string title = "Recurso não encontrado"
    )
    {
        ProblemDetails problemDetails = ProblemDetailsFactory.Create(
            StatusCodes.Status404NotFound,
            title,
            errorMessage,
            controller.HttpContext.Request.Path
        );

        return controller.NotFound(problemDetails);
    }

    public static IActionResult ToConflictProblem(
        this ControllerBase controller,
        string errorMessage,
        string title = "Conflito de dados"
    )
    {
        ProblemDetails problemDetails = ProblemDetailsFactory.Create(
            StatusCodes.Status409Conflict,
            title,
            errorMessage,
            controller.HttpContext.Request.Path
        );

        return controller.Conflict(problemDetails);
    }

    public static IActionResult ToOkResponse<T>(
        this ControllerBase controller,
        T data,
        string message = "Sucesso",
        int statusCode = StatusCodes.Status200OK
    )
    {
        ApiSuccessResponse<T> response = ApiResponseFactory.Success(data, message, statusCode);
        return controller.StatusCode(statusCode, response);
    }
}
