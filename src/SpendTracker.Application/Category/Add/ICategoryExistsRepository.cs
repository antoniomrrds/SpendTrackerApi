namespace SpendTracker.Application.Category.Add;

public interface ICategoryExistsRepository
{
    Task<bool> HasCategoryWithNameAsync(string name);
}