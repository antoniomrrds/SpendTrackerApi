using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Exceptions;

internal sealed partial class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,    
        Exception exception,
        CancellationToken cancellationToken)
    {
        LogGlobalException(exception);

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type  = exception.GetType().Name,
                Title ="An error occurred",
                Detail = exception.Message,
                Instance =  $"{httpContext.Request.Method} {httpContext.Request.Path}"
            }
        });
    }
    
    [LoggerMessage(Level = LogLevel.Error, Message = "Unhandled exception occured")]
    private partial void LogGlobalException(Exception exception);
}