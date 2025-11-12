using System.Globalization;
using Microsoft.AspNetCore.Localization;
using WebApi.Common.Web.Filters;
using WebApi.Features;
using WebApi.Infrastructure;
using DomainExceptionHandler = WebApi.Common.Web.Exceptions.DomainExceptionHandler;
using GlobalExceptionHandler = WebApi.Common.Web.Exceptions.GlobalExceptionHandler;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration).AddAllFeatures();

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

builder.Services.AddControllers();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseRequestLocalization(localizationOptions);

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

public abstract partial class Program;
