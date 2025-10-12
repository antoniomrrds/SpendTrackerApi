using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MySql;
using WebApi.E2E.Tests.Abstractions;

namespace WebApi.E2E.Tests.Factories;

public class MySqlTestWebAppFactory: WebApplicationFactory<Program>, ITestWebAppFactory , IAsyncLifetime
{
    private const string MySqlVersionString = "8.0.43";
    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0.43-debian")
        .WithUsername("test")
        .WithPassword("test")
        .WithDatabase("spendtracker")
        .Build();

    public async ValueTask InitializeAsync() => await _mySqlContainer.StartAsync();
    public new async Task DisposeAsync()
    {
        await _mySqlContainer.StopAsync();
        await _mySqlContainer.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(s =>
        {
            s.RemoveAll<DbContextOptions<AppDbContext>>();
            string connection = _mySqlContainer.GetConnectionString();
            MySqlServerVersion serverVersion = new(new Version(MySqlVersionString));
            s.AddDbContext<AppDbContext>(o => o.UseMySql(connection, serverVersion));
        });
    }

    public IServiceScope CreateScope() => Services.CreateScope();
}