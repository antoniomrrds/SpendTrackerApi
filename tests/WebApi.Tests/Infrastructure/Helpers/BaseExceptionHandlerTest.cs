using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute.ExceptionExtensions;
using TestUtilities.Common;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Helpers.Abstractions;

namespace WebApi.Tests.Infrastructure.Helpers;

public abstract class BaseExceptionHandlerTest<TFactory>
    : TestCommon,
        IClassFixture<TFactory>,
        IAsyncLifetime
    where TFactory : class, IExceptionTestWebAppFactory
{
    protected HttpClient HttpClient { get; private set; } = null!;
    protected TFactory Factory { get; }

    protected BaseExceptionHandlerTest(TFactory factory)
    {
        Factory = factory;
    }

    public async ValueTask InitializeAsync()
    {
        ConfigureTestServices();
        HttpClient = Factory.CreateClient();
        await Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        HttpClient?.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected abstract void ConfigureTestServices();

    protected void ConfigureMockUseCase<TUseCase>(
        TUseCase mockInstance,
        Action<TUseCase>? configureMock = null
    )
        where TUseCase : class
    {
        Factory.ConfigureTestServices(services =>
        {
            services.RemoveAll<TUseCase>();
            configureMock?.Invoke(mockInstance);
            services.AddScoped(_ => mockInstance);
        });
    }

    protected static void SetupUseCaseToThrow(
        ICreateCategoryUseCase mockUseCase,
        Exception exception
    )
    {
        mockUseCase
            .Perform(
                AnyParameterForMock<CreateCategoryInput>(),
                AnyParameterForMock<CancellationToken>()
            )
            .Throws(exception);
    }
}
