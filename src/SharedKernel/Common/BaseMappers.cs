using System.Globalization;

namespace SharedKernel.Common;

public static class BaseMappers
{
    public static string FormatDate(DateTime data) =>
        data.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

    public static string FormatCurrency(decimal amount) =>
        amount.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
}
