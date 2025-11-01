using Application.Categories.Common;
using Domain.Categories;
using Domain.Tests.Categories;

namespace Application.Tests.Categories.Common;

public static class CategoryDtoMock
{
    public static CategoryDto Valid()
    {
        Category category = MockCategory.Valid();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }
}
