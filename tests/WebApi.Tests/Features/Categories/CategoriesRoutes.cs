namespace WebApi.Tests.Features.Categories;

public static class CategoriesRoutes
{
    public const string BasePath = "/api/categories";

    public static Uri Add => new(BasePath, UriKind.Relative);

    public static Uri GetById(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Update(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Delete(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri GetAll => new(BasePath, UriKind.Relative);
}
