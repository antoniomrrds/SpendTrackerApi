using FluentValidation;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.GetById;

namespace WebApi.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddAllFeatures(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();

        return services;
    }
}
