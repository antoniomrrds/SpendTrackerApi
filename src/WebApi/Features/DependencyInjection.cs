using FluentValidation;
using Scrutor;
using SharedKernel.Abstractions;
using WebApi.Common.Extensions;

namespace WebApi.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddAllFeatures(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        RegisterUseCases(services);
        return services;
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        services.RegisterByMarker<IUseCaseMarker>();
    }
}
