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
        //Arrange
        await _sut.AddAsync(Category, _ct);
        await DbContext.SaveChangesAsync(_ct);
        //Act
        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == Category.Id,
            _ct
        );
        //Assert
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

    [Fact]
    public async Task UpdateAsync_WhenCategoryExists_ShouldReturnTrue()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        Category updatedCategory = new(Category.Id, "Novo Nome Atualizado", "Nova Descrição");
        string originalName = Category.Name;

        //Act
        bool isUpdated = await _sut.UpdateAsync(updatedCategory, _ct);
        Category categoryFromDb = await DbContext
            .Categories.AsNoTracking()
            .FirstAsync(c => c.Id == Category.Id, _ct);
        //Assert
        isUpdated.ShouldBeTrue();
        categoryFromDb.Name.ShouldBe("Novo Nome Atualizado");
        categoryFromDb.Description.ShouldBe("Nova Descrição");
        categoryFromDb.Name.ShouldNotBe(originalName);
    }
}
