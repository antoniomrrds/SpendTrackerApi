using Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using WebApi.E2E.Tests.Abstractions;

namespace WebApi.E2E.Tests.Factories;

public class SqliteTestWebAppFactory : WebApplicationFactory<Program>, ITestWebAppFactory,IAsyncLifetime
{
    private readonly SqliteConnection _connection;
    
    public SqliteTestWebAppFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ServiceDescriptor? dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                    typeof(IDbContextOptionsConfiguration<
                                                                        AppDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            ServiceDescriptor? dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                                      typeof(DbConnection));

            if (dbConnectionDescriptor != null)
            {
                services.Remove(dbConnectionDescriptor);
            }

            services.AddSingleton<DbConnection>(_ =>
            {
                _connection.Open();

                return _connection;
            });

            services.AddDbContext<AppDbContext>((container, options) =>
            {
                DbConnection connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }
    public IServiceScope CreateScope() => Services.CreateScope();
    public async ValueTask InitializeAsync()
    {
        using IServiceScope scope = Services.CreateScope();
        AppDbContext ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await ctx.Database.EnsureCreatedAsync();
        // await ctx.Database.MigrateAsync();
    }
    
    public new async Task DisposeAsync()
    {
        using IServiceScope scope = Services.CreateScope();
        AppDbContext ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await ctx.Database.EnsureDeletedAsync();
        await _connection.CloseAsync();                  
        await _connection.DisposeAsync();              
    }
}

