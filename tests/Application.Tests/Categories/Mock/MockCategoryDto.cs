using Application.Categories.Common;
using Domain.Categories;
using Domain.Tests.Categories;

namespace Application.Tests.Categories.Mock;

public static class MockCategoryDto
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
