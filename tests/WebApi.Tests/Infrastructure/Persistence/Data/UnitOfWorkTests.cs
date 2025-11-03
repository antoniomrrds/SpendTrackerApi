using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Data;

[Trait("Type", "Integration")]
public class UnitOfWorkTests : IClassFixture<SqliteInMemoryFixture>
{
    private readonly AppDbContext _context;
    private readonly UnitOfWork _sut;

    public UnitOfWorkTests(SqliteInMemoryFixture fixture)
    {
        _context = fixture.Context;
        _sut = new UnitOfWork(_context);
    }

    [Fact]
    public async Task CommitAsync_WhenCalled_ShouldPersistChanges()
    {
        Category category = new("Test", "Descrição");
        await _context.Categories.AddAsync(category, TestContext.Current.CancellationToken);
        await _sut.CommitAsync();
        Category? saved = await _context.Categories.FirstOrDefaultAsync(
            c => c.Id == category.Id,
            TestContext.Current.CancellationToken
        );
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe("Test");
    }
}
