using System.Globalization;

namespace SharedKernel.Resources;

public static class ValidationMessageProvider
{
    public static string Get(string key, params object[] args)
    {
        string? template = ValidationMessages.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);
        return string.Format(CultureInfo.CurrentCulture, template ?? $"[Missing:{key}]", args);
    }
}
