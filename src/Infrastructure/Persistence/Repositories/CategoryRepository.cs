using Application.Categories.Add;
using Domain.Categories;

namespace Infrastructure.Repositories;

internal class CategoryRepository:ICategoryRepository
{
    public Task<bool> HasCategoryWithNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<Category> AddAsync(Category category)
    {
        throw new NotImplementedException();
    }
}