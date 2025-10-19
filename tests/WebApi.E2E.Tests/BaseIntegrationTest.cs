using Infrastructure.Persistence.Data;
using Microsoft.Extensions.DependencyInjection;
using WebApi.E2E.Tests.Abstractions;

namespace WebApi.E2E.Tests;

public abstract class BaseIntegrationTest<TFactory> : IClassFixture<TFactory> where TFactory : class, ITestWebAppFactory
{
    
    protected BaseIntegrationTest(TFactory factory)
    {
        IServiceScope scope = factory.CreateScope();
        HttpClient = factory.CreateClient();
        Faker = FakerHelper.Faker;
        DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
   
    protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;
    protected HttpClient HttpClient { get; }
    protected Faker Faker { get; }
    protected AppDbContext DbContext { get; }
    
    protected async Task ResetDatabaseAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
    }

}

