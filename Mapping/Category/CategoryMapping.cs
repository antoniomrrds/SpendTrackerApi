using Mapster;

using SpendTrackApi.Controllers;
using SpendTrackApi.Controllers.Category;

namespace SpendTrackApi.Mapping.Category;

internal sealed class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Category, CategoryResponse>().TwoWays();
        config.NewConfig<CategoryRequest, Models.Category>().TwoWays();
    }
}
