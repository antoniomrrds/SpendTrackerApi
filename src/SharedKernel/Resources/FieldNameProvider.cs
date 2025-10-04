using System.Globalization;

namespace SharedKernel.Resources;

public static class FieldNameProvider
{
    public static string Get(string key)
    {
        return FieldNames.ResourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
    }
}