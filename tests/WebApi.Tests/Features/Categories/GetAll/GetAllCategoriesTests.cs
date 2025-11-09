using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

namespace WebApi.Tests.Features.Categories.GetAll;

[Trait("Type", "E2E")]
public class GetAllCategoriesTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private readonly List<Category> _categories;
    private readonly CancellationToken _ct = CancellationToken.None;

    public GetAllCategoriesTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _categories = CategoryFixture.GetCategories(3, true);
    }

    [Fact]
    public async Task GetAllCategories_WhenCategoryExists_ReturnsCategoryDto()
    {
        //Arrange
        await ResetDatabaseAsync();
        await CategorySeeder.AddRangeAsync(DbContext, _categories);
        //Action
        IReadOnlyList<CategoryDto> response = await HttpClient.GetAllCategoriesAndReturnDto(_ct);
        //Assert
        response.Count.ShouldBe(_categories.Count);
        response.ShouldAllBe(dto =>
            _categories.Any(c =>
                c.Id == dto.Id && c.Name == dto.Name && c.Description == dto.Description
            )
        );
    }
}
