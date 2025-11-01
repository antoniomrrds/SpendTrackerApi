using Domain.Categories;
using Domain.Tests.Categories;
using WebApi.Controllers.Categories.Add;

namespace Application.Tests.Categories.Add;

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
