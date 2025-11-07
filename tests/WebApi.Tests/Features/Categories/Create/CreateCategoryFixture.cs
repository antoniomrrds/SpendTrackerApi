using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Create;

public static class CreateCategoryFixture
{
    public static CreateCategoryRequest ValidRequest(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);

        return new CreateCategoryRequest(category.Name, category.Description ?? string.Empty);
    }

    public static CreateCategoryRequest Invalid() =>
        new(Name: string.Empty, Description: "             ");

    public static CreateCategoryInput CategoryInput(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);
        return new CreateCategoryInput() { Name = category.Name, Description = category.Description };
    }
}
