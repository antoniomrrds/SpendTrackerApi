using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Categories.Add;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name);
}