using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests.Abstractions;

public interface ITestWebAppFactory
{
    HttpClient CreateClient();
    IServiceScope CreateScope();
}
