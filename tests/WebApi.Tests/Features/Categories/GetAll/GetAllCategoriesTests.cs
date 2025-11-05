using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Helpers.Factories;

namespace WebApi.Tests.Features.Categories.GetAll;

[Trait("Type", "E2E")]
public class GetAllCategoriesTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private readonly List<Category> _categories;

    public GetAllCategoriesTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _categories = CategoryFixture.GetCategories(3, true);
    }

    private async Task SeedCategoriesAsync()
    {
        await DbContext.Categories.AddRangeAsync(_categories);
        await DbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAllCategories_WhenCategoryExists_ReturnsCategoryDto()
    {
        //Arrange
        await SeedCategoriesAsync();
        //Action
        IReadOnlyList<CategoryDto> response = await HttpClient.GetAllCategoriesAndReturnDto(
            CancellationToken
        );
        //Assert
        response.Count.ShouldBe(_categories.Count);
        response.ShouldAllBe(dto =>
            _categories.Any(c =>
                c.Id == dto.Id && c.Name == dto.Name && c.Description == dto.Description
            )
        );
    }
}
