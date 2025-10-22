using Microsoft.Extensions.DependencyInjection;

namespace WebApi.E2E.Tests.Abstractions;

public interface ITestWebAppFactory
{
    HttpClient CreateClient();
    IServiceScope CreateScope();
}
