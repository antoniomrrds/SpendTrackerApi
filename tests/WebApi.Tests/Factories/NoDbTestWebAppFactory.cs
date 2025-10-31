using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests.Factories;

public class NoDbTestWebAppFactory : WebApplicationFactory<Program>
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
}
