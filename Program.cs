using System.Reflection;

using FluentValidation;

using Mapster;

using MapsterMapper;

using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

using SpendTrackApi.Data;
using SpendTrackApi.Mapping;
using SpendTrackApi.Mapping.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Add services to the container.
builder.Services.AddControllers();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                          ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

var config = TypeAdapterConfig.GlobalSettings;

config.RegisterMappings();

new MapsterConfig().Register(config);

builder.Services.AddSingleton(config);

builder.Services.AddScoped<IMapper, Mapper>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
