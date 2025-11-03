using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests.Helpers.Abstractions;

public interface ITestWebAppFactory
{
    HttpClient CreateClient();
    IServiceScope CreateScope();
}
