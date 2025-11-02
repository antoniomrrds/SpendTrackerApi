using WebApi.Constants;

namespace WebApi.Helper;

public static class ProblemDetailsTypeHelper
{
    public static string GetDefaultType(int statusCode) =>
        statusCode switch
        {
            400 => ProblemDetailsTypes.BadRequest,
            404 => ProblemDetailsTypes.NotFound,
            409 => ProblemDetailsTypes.Conflict,
            _ => ProblemDetailsTypes.InternalServerError,
        };
}
