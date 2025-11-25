using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Repositories.Categories;
using WebApi.Tests.Infrastructure.Helpers.db;

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
        //Arrange
        await _sut.AddAsync(Category, Ct);
        await DbContext.SaveChangesAsync(Ct);
        //Act
        Category? saved = await DbContext.Categories.FirstOrDefaultAsync(
            c => c.Id == Category.Id,
            Ct
        );
        //Assert
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(Category.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenCategoryDoesNotExist_ShouldReturnFalse()
    {
        //Act
        bool isUpdated = await _sut.UpdateAsync(Category, Ct);
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
        bool isUpdated = await _sut.UpdateAsync(updatedCategory, Ct);
        Category categoryFromDb = await DbContext
            .Categories.AsNoTracking()
            .FirstAsync(c => c.Id == Category.Id, Ct);
        //Assert
        isUpdated.ShouldBeTrue();
        categoryFromDb.Name.ShouldBe("Novo Nome Atualizado");
        categoryFromDb.Description.ShouldBe("Nova Descrição");
        categoryFromDb.Name.ShouldNotBe(originalName);
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryToDeleteIsNotFound_ShouldReturnFalse()
    {
        //Act
        bool isDeleted = await _sut.DeleteAsync(Category.Id, Ct);
        //Assert
        isDeleted.ShouldBeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WhenCategoryExists_ShouldDeleteCategoryAndReturnTrue()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        //Act
        bool isDeleted = await _sut.DeleteAsync(Category.Id, Ct);
        //Assert
        isDeleted.ShouldBeTrue();
        Category? categoryFromDb = await DbContext
            .Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == Category.Id, Ct);
        categoryFromDb.ShouldBeNull();
    }
}
