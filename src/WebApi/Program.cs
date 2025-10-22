using Application;
using Infrastructure;
using Microsoft.AspNetCore.Localization;
using Scalar.AspNetCore;
using System.Globalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

// Add services to the container.
// var config = TypeAdapterConfig.GlobalSettings;
//
// config.Scan(typeof(Program).Assembly);
//
// builder.Services.AddSingleton(config);
//
// builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; });
WebApplication app = builder.Build();

CultureInfo cultureInfo = new("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

RequestLocalizationOptions localizationOptions = new()
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = [cultureInfo],
    SupportedUICultures = [cultureInfo]
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public abstract partial class Program;
