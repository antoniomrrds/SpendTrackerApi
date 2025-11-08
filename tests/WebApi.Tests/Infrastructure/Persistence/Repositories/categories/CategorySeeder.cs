using WebApi.Domain.Categories;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

internal static class CategorySeeder
{
    public static async Task AddAsync(AppDbContext context, Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }

    public static async Task AddRangeAsync(AppDbContext context, IEnumerable<Category> categories)
    {
        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();
    }
}
