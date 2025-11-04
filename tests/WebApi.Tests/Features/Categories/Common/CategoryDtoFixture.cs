using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Domain.Categories;

namespace WebApi.Tests.Features.Categories.Common;

internal static class CategoryDtoFixture
{
    public static CategoryDto GetCategoryDto(bool useNewSeed = false)
    {
        Category category = CategoryFixture.GetCategory(useNewSeed);
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }

    public static List<CategoryDto> GetCategoriesDto(int count = 3, bool useNewSeed = false)
    {
        List<Category> categories = CategoryFixture.GetCategories(count, useNewSeed);
        return
        [
            .. categories.Select(c => new CategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            }),
        ];
    }

    public static IReadOnlyList<CategoryDto> EmptyList() => [];
}
