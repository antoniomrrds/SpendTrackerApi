using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;
using WebApi.E2E.Tests.Abstractions;

namespace WebApi.E2E.Tests.Factories;

public class SqliteTestWebAppFactory : WebApplicationFactory<Program>, ITestWebAppFactory
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<DbConnection>();

            services.AddSingleton<DbConnection>(_ =>
            {
                SqliteConnection connection = new("DataSource=:memory:");
                connection.Open();
                return connection;
            });

            services.AddDbContext<AppDbContext>((provider, options) =>
            {
                DbConnection connection = provider.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }

    public IServiceScope CreateScope() => Services.CreateScope();
}
