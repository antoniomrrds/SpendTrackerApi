using WebApi.Common.Web.Extensions;
using WebApi.Common.Web.Helper;

namespace WebApi.Common.Web.Factories;

public static class CustomProblemDetailsFactory
{
    public static ProblemDetails Create(
        HttpContext context,
        int statusCode,
        string title,
        string detail,
        string? instance = null,
        string? type = null!
    )
    {
        return new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = instance ?? context.GetRequestSignature(),
            Type = type ?? ProblemDetailsTypeHelper.GetDefaultType(statusCode),
        };
    }
}
