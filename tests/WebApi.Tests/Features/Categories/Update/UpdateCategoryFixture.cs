using WebApi.Domain.Categories;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Update;

public static class UpdateCategoryFixture
{
    public static UpdateCategoryInput UpdateInput(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);
        return new UpdateCategoryInput()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }
}
