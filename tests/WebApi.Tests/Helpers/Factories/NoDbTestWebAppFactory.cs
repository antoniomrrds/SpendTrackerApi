using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Tests.Helpers.Abstractions;

namespace WebApi.Tests.Helpers.Factories;

public class NoDbTestWebAppFactory : WebApplicationFactory<Program>, IExceptionTestWebAppFactory
{
    public Action<IServiceCollection>? ConfigureTestServicesAction { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ConfigureTestServicesAction?.Invoke(services);
        });

        builder.UseEnvironment("Development");
    }

    public void ConfigureTestServices(Action<IServiceCollection> configureAction)
    {
        ConfigureTestServicesAction = configureAction;
    }
}
