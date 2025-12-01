using Microsoft.EntityFrameworkCore;
using WebApi.Common.Extensions;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Infrastructure.Persistence.Repositories;

namespace WebApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        const string mySqlVersionString = "8.0.43";

        string connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found."
            );
        MySqlServerVersion serverVersion = new(new Version(mySqlVersionString));
        services.AddDbContext<AppDbContext>(o =>
            o.UseMySql(connectionString, serverVersion).EnableDetailedErrors()
        );

        RegisterRepositories(services);
        return services;
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.RegisterByMarker<IBaseRepositoryMarker>();
    }
}
