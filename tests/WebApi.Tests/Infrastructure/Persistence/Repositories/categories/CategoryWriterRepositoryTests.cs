using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Repositories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

[Trait("Type", "Integration")]
public class CategoryWriterRepositoryTests : CategoryIntegrationTestBase
{
    private readonly CategoryWriterRepository _sut;

    public CategoryWriterRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new CategoryWriterRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_WhenCategoryIsValid_ShouldPersistCategory()
    {
        await _sut.AddAsync(Category, AnyCancellationToken);
        await DbContext.SaveChangesAsync(AnyCancellationToken);

        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == Category.Id,
            CancellationToken
        );
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(Category.Name);
    }
}
