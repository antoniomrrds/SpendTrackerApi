using Application.Abstractions.Data;
using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.Categories.Add; 
using Infrastructure.Persistence.Repositories;
using MySqlConnector; // Adicionar para usar o MySqlDataSource se necessário para o MySql

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    { 
        const string mySqlVersionString = "8.0.43";

        string connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        
        MySqlServerVersion serverVersion = new(new Version(mySqlVersionString));
        services.AddDbContext<AppDbContext>(o => o.UseMySql(connectionString, serverVersion));
           
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}