using System.Globalization;
using SharedKernel.Extensions;

namespace SharedKernel.Common;

public static class BaseMappers
{
    public static string ToFormattedDate(this DateTime date) => date.ToPtBrDateTime();

    public static string ToFormattedDate(this DateTime? date) =>
        date?.ToPtBrDateTime() ?? string.Empty;

    public static string ToFormattedCurrency(this decimal amount) =>
        amount.ToString("C2", CultureInfo.GetCultureInfo("pt-BR"));

    public static string ToFormattedCurrency(this decimal? amount) =>
        amount?.ToString("C2", CultureInfo.GetCultureInfo("pt-BR")) ?? "R$ 0,00";
}
