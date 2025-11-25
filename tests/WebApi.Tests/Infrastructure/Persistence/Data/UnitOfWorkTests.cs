using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Helpers.db;

namespace WebApi.Tests.Infrastructure.Persistence.Data;

[Trait("Type", "Integration")]
public class UnitOfWorkTests : BaseSqliteIntegrationTest
{
    private readonly UnitOfWork _sut;
    private readonly Category _getCategory;
    private readonly CancellationToken _ct = CancellationToken.None;

    public UnitOfWorkTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new UnitOfWork(DbContext);
        _getCategory = CategoryFixture.GetCategory();
    }

    [Fact]
    public async Task CommitAsync_WhenCalled_ShouldPersistChanges()
    {
        await DbContext.Categories.AddAsync(_getCategory, _ct);
        await _sut.CommitAsync(_ct);
        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == _getCategory.Id,
            _ct
        );
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(_getCategory.Name);
    }
}
