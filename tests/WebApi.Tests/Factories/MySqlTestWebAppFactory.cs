using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using WebApi.Tests.Abstractions;

namespace WebApi.Tests.Factories;

public class MySqlTestWebAppFactory: WebApplicationFactory<Program>, ITestWebAppFactory , IAsyncLifetime
{
    private const string MySqlVersionString = "8.0.43";
    private readonly MySqlContainer _mySqlContainer;

    public MySqlTestWebAppFactory()
    {
        _mySqlContainer  = new MySqlBuilder()
            .WithImage("mysql:8.0.43-debian")
            .WithUsername("test")
            .WithPassword("test")
            .WithDatabase("spendtracker")
            .Build();
    }

    public async ValueTask InitializeAsync()
    {
        await _mySqlContainer.StartAsync();
        using IServiceScope scope = Services.CreateScope();
        IServiceProvider scopedServices = scope.ServiceProvider;
        AppDbContext cntx = scopedServices.GetRequiredService<AppDbContext>();
        await cntx.Database.EnsureCreatedAsync();

    }

    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        await _mySqlContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string? connectionString = _mySqlContainer.GetConnectionString();
        base.ConfigureWebHost(builder);
        builder.ConfigureTestServices(services =>
        {
           MySqlServerVersion serverVersion = new(new Version(MySqlVersionString));

            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion);
            });
        });
    }
    public IServiceScope CreateScope() => Services.CreateScope();
}