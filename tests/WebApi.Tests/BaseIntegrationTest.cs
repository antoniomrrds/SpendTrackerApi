using Microsoft.Extensions.DependencyInjection;
using TestUtilities.Common;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Helpers.Abstractions;

namespace WebApi.Tests;

public abstract class BaseIntegrationTest<TFactory> : TestCommon, IClassFixture<TFactory>
    where TFactory : class, ITestWebAppFactory
{
    protected HttpClient HttpClient { get; }
    protected AppDbContext DbContext { get; init; }

    protected BaseIntegrationTest(TFactory factory)
    {
        IServiceScope scope = factory.CreateScope();
        DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        HttpClient = factory.CreateClient();
    }

    protected async Task ResetDatabaseAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
    }
}
