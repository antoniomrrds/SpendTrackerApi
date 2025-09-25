using Mapster;
using SpendTracker.Api.Controllers.Category;

namespace SpendTracker.Api.Mapping.Category;

internal sealed class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Category, CategoryResponse>().TwoWays();
        config.NewConfig<CategoryRequest, Models.Category>().TwoWays();
    }
}
