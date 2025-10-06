using Domain.Categories;
using Infrastructure.Persistence.Data;
using Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Persistence.Data;

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
        // Arrange
        var category = new Category("Test", "Descrição");
        await _context.Categories.AddAsync(category ,  TestContext.Current.CancellationToken);
        // Act
        await _sut.CommitAsync();
        // Assert
        var saved = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id, TestContext.Current.CancellationToken);
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe("Test");
    }
}
