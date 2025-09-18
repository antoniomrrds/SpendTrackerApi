using Mapster;

using SpendTrackApi.Controllers;

namespace SpendTrackApi.Mapping.Category;

public class CategoryMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Category, CategoryResponse>();
    }
}
