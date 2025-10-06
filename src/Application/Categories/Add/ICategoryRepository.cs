using Domain.Categories;

namespace Application.Categories.Add;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name , CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
}