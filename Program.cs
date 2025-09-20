using System.Reflection;

using FluentValidation;

using Mapster;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

using SpendTrackApi.Data;
using SpendTrackApi.Mapping.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.AddControllers();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

TypeAdapterConfig config = TypeAdapterConfig.GlobalSettings;

config.Scan(typeof(Program).Assembly);
// config.RegisterMappings();

builder.Services.AddSingleton(config);

builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddDbContext<AppDbContext>(options => { options.UseSqlite(connectionString); });

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Configures routing to generate lowercase URLs and query strings.
// This helps avoid issues on case-sensitive environments (e.g., Linux servers),
// Example:
//   Without this setting:    /api/Tarefas/5?SortBy=Nome
//   With this setting:    /api/tarefas/5?sortby=nome
// This ensures consistency, prevents 404 errors in production,
// and follows REST API best practices by using clean, predicatable URLs.
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true; // Forces lowercase URls
    options.LowercaseQueryStrings = true; // Forces lowercase query strings
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
