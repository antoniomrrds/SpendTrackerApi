using Microsoft.AspNetCore.Diagnostics;
using WebApi.Factories;

namespace WebApi.Exceptions;

internal sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        LogGlobalException(exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = ProblemDetailsFactory.Create(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "An error occurred",
                    detail: exception.Message,
                    instance: $"{httpContext.Request.Method} {httpContext.Request.Path}",
                    type: exception.GetType().Name
                ),
            }
        );
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled exception occured")]
    private partial void LogGlobalException(Exception exception);
}
