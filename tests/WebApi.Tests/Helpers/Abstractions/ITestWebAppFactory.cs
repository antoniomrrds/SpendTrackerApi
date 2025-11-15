using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Tests.Helpers.Abstractions;

public interface IBaseTestWebAppFactory
{
    HttpClient CreateClient();
}

public interface ITestWebAppFactory : IBaseTestWebAppFactory
{
    IServiceScope CreateScope();
}

public interface IExceptionTestWebAppFactory : IBaseTestWebAppFactory
{
    void ConfigureTestServices(Action<IServiceCollection> configureAction);
}
