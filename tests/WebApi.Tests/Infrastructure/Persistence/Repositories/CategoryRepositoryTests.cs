using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories;

[Trait("Type", "Integration")]
public class CategoryRepositoryTests : BaseSqliteIntegrationTest
{
    private readonly CategoryRepository _sut;
    private readonly Category _category;

    private async Task SeedCategoryAsync()
    {
        await DbContext.Categories.AddAsync(_category);
        await DbContext.SaveChangesAsync();
    }

    public CategoryRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _category = CategoryFixture.GetCategory(true);
        _sut = new CategoryRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_WhenCategoryIsValid_ShouldPersistCategory()
    {
        await SeedCategoryAsync();

        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == _category.Id,
            CancellationToken
        );
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(_category.Name);
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameExists_ShouldReturnTrue()
    {
        await SeedCategoryAsync();
        bool saved = await _sut.HasCategoryWithNameAsync(_category.Name, CancellationToken);
        saved.ShouldBeTrue();
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameDoesNotExist_ShouldReturnFalse()
    {
        bool exists = await _sut.HasCategoryWithNameAsync("NameDoesNotExit", CancellationToken);
        exists.ShouldBeFalse();
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryExists_ShouldReturnCategoryDto()
    {
        await ResetDatabaseAsync();
        await SeedCategoryAsync();
        CategoryDto? categoryResponse = await _sut.GetByIdAsync(_category.Id, CancellationToken);

        categoryResponse.ShouldNotBeNull();
        categoryResponse.ShouldSatisfyAllConditions(
            c => c.ShouldNotBeNull(),
            c => c.Id.ShouldBe(_category.Id),
            c => c.Name.ShouldBe(_category.Name),
            c => c.Description.ShouldBe(_category.Description)
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenCategoryDoesNotExist_ShouldReturnNull()
    {
        CategoryDto? categoryResponse = await _sut.GetByIdAsync(_category.Id, CancellationToken);

        categoryResponse.ShouldBeNull();
    }
}
