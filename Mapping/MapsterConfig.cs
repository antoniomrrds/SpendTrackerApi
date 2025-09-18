using Mapster;

using SpendTrackApi.Controllers;

using Entity = SpendTrackApi.Models;

namespace SpendTrackApi.Mapping;

public class MapsterConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Entity.Category, CategoryResponse>();

        // Se quiser customizar, por exemplo:
        // config.NewConfig<Category, CategoryResponse>()
        //       .Map(dest => dest.Name, src => src.CategoryName);
    }
}
