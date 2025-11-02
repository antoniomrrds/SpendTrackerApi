namespace WebApi.Extensions;

public static class HttpContextExtensions
{
    public static string GetRequestSignature(this HttpContext context)
    {
        return $"{context.Request.Method} {context.Request.Path}";
    }
}
