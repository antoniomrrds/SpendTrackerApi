using WebApi.Domain.Categories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

// [Collection("IntegrationTests")]
public abstract class CategoryIntegrationTestBase : BaseSqliteIntegrationTest
{
    protected CategoryIntegrationTestBase(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        Category = CategoryFixture.GetCategory(true);
        Categories = CategoryFixture.GetCategories(3, true);
    }

    protected Category Category { get; set; }
    protected IEnumerable<Category> Categories { get; set; }

    protected async Task<Category> MakeCreateCategoryAsync()
    {
        await CategorySeeder.AddAsync(DbContext, Category);
        return Category;
    }

    protected async Task<IEnumerable<Category>> MakeCreateCategoriesAsync()
    {
        await CategorySeeder.AddRangeAsync(DbContext, Categories);
        return Categories;
    }
}
