namespace WebApi.Tests.Features.Expenses;

public static class ExpenseRoutes
{
    public const string BasePath = "/api/expenses";

    public static Uri Add => new(BasePath, UriKind.Relative);

    public static Uri GetById(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Update(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri Delete(Guid id) => new($"{BasePath}/{id}", UriKind.Relative);

    public static Uri GetAll => new(BasePath, UriKind.Relative);
}
