using WebApi.Domain.Categories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Helpers.db;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

public abstract class CategoryIntegrationTestBase : BaseSqliteIntegrationTest, IAsyncLifetime
{
    protected CategoryIntegrationTestBase(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        Category = CategoryFixture.GetCategory(true);
        Categories = CategoryFixture.GetCategories(3, true);
    }

    public async ValueTask InitializeAsync()
    {
        await ResetDatabaseAsync();
        DbContext.ChangeTracker.Clear();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
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
