using Domain.Errors;
using Microsoft.AspNetCore.Diagnostics;
using WebApi.Factories;

namespace WebApi.Exceptions;

internal sealed partial class DomainExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<DomainExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        if (exception is not DomainException domainException)
        {
            return false;
        }

        LogDomainException(domainException);

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = CustomProblemDetailsFactory.Create(
                    httpContext,
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "Business rule violation",
                    detail: exception.Message
                ),
            }
        );
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Domain exception handled")]
    private partial void LogDomainException(DomainException exception);
}
