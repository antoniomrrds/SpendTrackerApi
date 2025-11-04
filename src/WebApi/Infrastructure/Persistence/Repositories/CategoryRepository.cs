using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> HasCategoryWithNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Categories.AnyAsync(c => c.Name == name, cancellationToken);
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<CategoryDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default
    )
    {
        return await _context
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
        return await _context
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
