using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

[Trait("Type", "Integration")]
public class CategoryReaderRepositoryTests : CategoryIntegrationTestBase
{
    private readonly CategoryReaderRepository _sut;

    public CategoryReaderRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new CategoryReaderRepository(DbContext);
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategoryDto()
    {
        //Arrange
        await ResetDatabaseAsync();
        await MakeCreateCategoryAsync();
        //Act
        CategoryDto? categoryResponse = await _sut.GetByIdAsync(Category.Id, CancellationToken);
        //Assert
        categoryResponse.ShouldNotBeNull();
        categoryResponse.ShouldSatisfyAllConditions(
            c => c.ShouldNotBeNull(),
            c => c.Id.ShouldBe(Category.Id),
            c => c.Name.ShouldBe(Category.Name),
            c => c.Description.ShouldBe(Category.Description)
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        //Act
        CategoryDto? categoryResponse = await _sut.GetByIdAsync(Category.Id, CancellationToken);
        //Assert
        categoryResponse.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_WhenCategoriesDoesNotExist_ShouldReturnEmptyList()
    {
        //Arrange
        await ResetDatabaseAsync();
        //Act
        IEnumerable<CategoryDto> categories = await _sut.GetAllAsync(CancellationToken);
        //Assert
        categories.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WhenCategoriesExists_ShouldReturnCategoriesDto()
    {
        //Arrange
        await ResetDatabaseAsync();
        await MakeCreateCategoriesAsync();
        //Act
        IEnumerable<CategoryDto> categories = await _sut.GetAllAsync(CancellationToken);
        //Assert
        List<CategoryDto> categoryList = [.. categories];
        categoryList.Count.ShouldBe(Categories.Count());
        categoryList.ShouldAllBe(dto =>
            Categories.Any(c =>
                c.Id == dto.Id && c.Name == dto.Name && c.Description == dto.Description
            )
        );
    }
}
