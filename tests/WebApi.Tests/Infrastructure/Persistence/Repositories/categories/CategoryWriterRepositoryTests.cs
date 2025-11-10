using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

[Trait("Type", "Integration")]
public class CategoryWriterRepositoryTests : CategoryIntegrationTestBase
{
    private readonly CategoryWriterRepository _sut;
    private readonly CancellationToken _ct = CancellationToken.None;

    public CategoryWriterRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new CategoryWriterRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_WhenCategoryIsValid_ShouldPersistCategory()
    {
        await _sut.AddAsync(Category, _ct);
        await DbContext.SaveChangesAsync(_ct);

        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == Category.Id,
            _ct
        );
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(Category.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        //Act
        bool isUpdated = await _sut.UpdateAsync(Category, _ct);
        //Assert
        isUpdated.ShouldBeFalse();
    }
}
