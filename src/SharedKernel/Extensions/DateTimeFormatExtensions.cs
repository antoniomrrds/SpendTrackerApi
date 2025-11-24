using System.Globalization;

namespace SharedKernel.Extensions;

public static class DateTimeExtensions
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");

    public static string ToPtBrDateTime(this DateTime value) =>
        value.ToString("dd/MM/yyyy HH:mm:ss", PtBr);
}
