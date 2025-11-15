using WebApi.Domain.Categories;
using WebApi.Features.Categories.Delete;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Delete;

internal static class DeleteCategoryFixture
{
    public static DeleteCategoryInput DeleteInput(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);
        return new DeleteCategoryInput() { Id = category.Id };
    }
}
