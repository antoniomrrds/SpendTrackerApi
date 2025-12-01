using System.Reflection;
using Scrutor;

namespace WebApi.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterByMarker<TMarker>(this IServiceCollection services)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();
        return services.RegisterByMarker<TMarker>(callingAssembly);
    }

    public static IServiceCollection RegisterByMarker<TMarker>(
        this IServiceCollection services,
        params Assembly[] assemblies
    )
    {
        if (assemblies.Length == 0)
        {
            assemblies = [typeof(TMarker).Assembly];
        }

        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<TMarker>(), publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }
}
