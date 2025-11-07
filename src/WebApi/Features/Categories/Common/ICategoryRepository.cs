using WebApi.Domain.Categories;

namespace WebApi.Features.Categories.Common;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(
        string name,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken = default);
}
