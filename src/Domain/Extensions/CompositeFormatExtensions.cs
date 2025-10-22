using System.Globalization;
using System.Text;

namespace SharedKernel.Extensions;

public static class CompositeFormatExtensions
{
    public static string FormatInvariant(this CompositeFormat format, params object[] args)
    {
        return string.Format(CultureInfo.InvariantCulture, format, args);
    }
}