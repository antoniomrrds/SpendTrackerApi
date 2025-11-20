using WebApi.Domain.Errors;
using static WebApi.Domain.Errors.ErrorPatterns;

namespace WebApi.Domain.Categories;

public static class CategoryErrorCodes
{
    private const string Prefix = "CATEGORY";
    public const string NotFound = $"{Prefix}.{ErrorPatterns.NotFound}";
    public const string NameAlreadyExists = $"{Prefix}.{AlreadyExists}";
}
