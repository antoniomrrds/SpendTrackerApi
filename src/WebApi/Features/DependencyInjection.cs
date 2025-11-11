using FluentValidation;
using WebApi.Features.Categories.Create;
using WebApi.Features.Categories.GetAll;
using WebApi.Features.Categories.GetById;
using WebApi.Features.Categories.Update;

namespace WebApi.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddAllFeatures(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddScoped<ICreateCategoryUseCase, CreateCategoryUseCase>();
        services.AddScoped<IGetCategoryByIdUseCase, GetCategoryByIdUseCase>();
        services.AddScoped<IUpdateCategoryUseCase, UpdateCategoryUseCase>();
        services.AddScoped<IGetAllCategoriesUseCase, GetAllCategoriesUseCase>();

        return services;
    }
}
