using Application.Categories.GetById;
using Domain.Categories;
using SharedKernel;

namespace Application.Categories.Common;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name , CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}