using System.Globalization;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Localization;
using Scalar.AspNetCore;
using WebApi.Exceptions;
using WebApi.Filters;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<ModelBindingEnvelopeFilter>();

builder.Services.AddProblemDetails(configure =>
{
    configure.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
    };
});

builder
    .Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

WebApplication app = builder.Build();

CultureInfo cultureInfo = new("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

RequestLocalizationOptions localizationOptions = new()
{
    DefaultRequestCulture = new RequestCulture(cultureInfo),
    SupportedCultures = [cultureInfo],
    SupportedUICultures = [cultureInfo],
};

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public abstract partial class Program;
