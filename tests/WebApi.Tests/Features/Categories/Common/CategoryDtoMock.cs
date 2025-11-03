using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Common;

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

    public static IEnumerable<CategoryDto> ValidList(int count = 3)
    {
        for (int i = 0; i < count; i++)
        {
            Category category = MockCategory.Valid();

            yield return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }
    }

    public static IEnumerable<CategoryDto> EmptyList()
    {
        return [];
    }
}
