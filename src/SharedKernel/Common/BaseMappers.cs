using System.Globalization;
using SharedKernel.Extensions;

namespace SharedKernel.Common;

public static class BaseMappers
{
    public static string FormatDate(DateTime data) => data.ToPtBrDateTime();

    public static string FormatCurrency(decimal amount) =>
        amount.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));
}
