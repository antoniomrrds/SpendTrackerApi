using Microsoft.EntityFrameworkCore;

using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories.Categories;

public class CategoryReaderRepository(AppDbContext context)
    : BaseRepository(context),
        ICategoryReaderRepository
{
    public async Task<CategoryDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await Context
            .Categories.AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            })
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await Context
            .Categories.AsNoTracking()
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
            })
            .ToListAsync(cancellationToken);
    }
}
