using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

using SpendTrackApi.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});






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
    options.LowercaseUrls = true;            // Forces lowercase URls
    options.LowercaseQueryStrings = true;    // Forces lowercase query strings
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

app.Run();
