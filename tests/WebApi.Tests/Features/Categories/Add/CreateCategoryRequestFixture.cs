using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Add;

public static class CreateCategoryRequestFixture
{
    public static CreateCategoryRequest Valid(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);

        return new CreateCategoryRequest(category.Name, category.Description ?? string.Empty);
    }

    public static CreateCategoryRequest Invalid() =>
        new(Name: string.Empty, Description: "             ");
}
