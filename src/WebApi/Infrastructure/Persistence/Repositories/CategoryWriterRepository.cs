using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories;

public class CategoryWriterRepository(AppDbContext context)
    : BaseRepository(context),
        ICategoryWriterRepository
{
    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await Context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<bool> UpdateAsync(
        Category category,
        CancellationToken cancellationToken = default
    )
    {
        int affectedRows = await Context
            .Categories.Where(c => c.Id == category.Id)
            .ExecuteUpdateAsync(
                setters =>
                    setters
                        .SetProperty(c => c.Name, category.Name)
                        .SetProperty(c => c.Description, category.Description),
                cancellationToken
            );
        return affectedRows > 0;
    }
}
