using Domain.Categories;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Persistence.Repositories;

//run the command to execute the integration test
//dotnet test --filter Type=Integration 
[Trait("Type", "Integration")]
public class CategoryRepositoryTests:IClassFixture<SqliteInMemoryFixture>
{
    private readonly AppDbContext _context;
    private readonly CategoryRepository _sut;
    private readonly Faker _faker = FakerHelper.Faker;
    private readonly Category _category;
    
    public CategoryRepositoryTests(SqliteInMemoryFixture fixture)
    {
        _context = fixture.Context;
        _category = new Category(_faker.Name.FirstName(), _faker.Lorem.Letter(200));
        _sut = new CategoryRepository(_context);

    }
    
    [Fact]
    public async Task AddAsync_WhenCategoryIsValid_ShouldPersistCategory()
    {
        await _sut.AddAsync(_category,TestContext.Current.CancellationToken);
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);

        Category? saved = await _context.Categories.FirstOrDefaultAsync(c => c.Id == _category.Id,
            cancellationToken: TestContext.Current.CancellationToken);
        saved.ShouldNotBeNull();
        saved.Name.ShouldBe(_category.Name);
    }

    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameExists_ShouldReturnTrue()
    {
        await _sut.AddAsync(_category ,TestContext.Current.CancellationToken );
        await _context.SaveChangesAsync(TestContext.Current.CancellationToken);
        bool saved = await _sut.HasCategoryWithNameAsync(_category.Name, TestContext.Current.CancellationToken);
        saved.ShouldBeTrue();
    }
    
    [Fact]
    public async Task HasCategoryWithNameAsync_WhenCategoryNameDoesNotExist_ShouldReturnFalse()
    {
        bool exists = await _sut.HasCategoryWithNameAsync("NameDoesNotExit", TestContext.Current.CancellationToken);
        exists.ShouldBeFalse();
    }

}