using Domain.Categories;

namespace Application.Categories.Add;

public interface ICategoryRepository
{
    Task<bool> HasCategoryWithNameAsync(string name);
    Task<Category> AddAsync(Category category);
}