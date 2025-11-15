using WebApi.Domain.Categories;

namespace WebApi.Features.Categories.Common;

public interface ICategoryReaderRepository
{
    Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
}

public interface ICategoryWriterRepository
{
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ICategoryCheckRepository
{
    Task<bool> HasCategoryWithNameAsync(
        string name,
        Guid? excludeId = null,
        CancellationToken cancellationToken = default
    );
}
