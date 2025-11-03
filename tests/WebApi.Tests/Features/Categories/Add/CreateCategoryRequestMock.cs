using WebApi.Domain.Categories;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Add;

public static class CreateCategoryRequestMock
{
    public static CreateCategoryRequest Valid()
    {
        Category category = MockCategory.Valid();

        return new CreateCategoryRequest(category.Name, category.Description ?? string.Empty);
    }

    public static CreateCategoryRequest Invalid() =>
        new(Name: string.Empty, Description: "             ");
}
