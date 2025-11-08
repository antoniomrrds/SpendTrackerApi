using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories;

public class CategoryCheckRepository(AppDbContext context)
    : BaseRepository(context),
        ICategoryCheckRepository
{
    public async Task<bool> HasCategoryWithNameAsync(
        string name,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Category> query = Context.Categories.Where(c => c.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
