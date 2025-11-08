using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

[Trait("Type", "Integration")]
public class CategoryCheckRepositoryTests : CategoryIntegrationTestBase
{
    private readonly CategoryCheckRepository _sut;

    public CategoryCheckRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new CategoryCheckRepository(DbContext);
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameExists_ShouldReturnTrue()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        //Act
        bool saved = await _sut.HasCategoryWithNameAsync(
            Category.Name,
            cancellationToken: CancellationToken
        );
        //Assert
        saved.ShouldBeTrue();
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameDoesNotExist_ShouldReturnFalse()
    {
        //Act
        bool exists = await _sut.HasCategoryWithNameAsync(
            "NameDoesNotExit",
            cancellationToken: CancellationToken
        );
        //Assert
        exists.ShouldBeFalse();
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryWithSameNameIsExcluded_ShouldReturnFalse()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        //Act
        bool exists = await _sut.HasCategoryWithNameAsync(
            Category.Name,
            excludeId: Category.Id,
            cancellationToken: CancellationToken
        );
        //Assert
        exists.ShouldBeFalse();
    }
}
