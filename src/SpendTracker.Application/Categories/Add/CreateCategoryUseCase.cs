using SharedKernel;
using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Categories.Add;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CreateCategoryResult>>  Perform(CreateCategoryCommand command)
    {
        var categoryAlreadyExists = await _categoryRepository.HasCategoryWithNameAsync(command.Name);
        if (categoryAlreadyExists)
        {
            return CategoryErrors.CategoryNameAlreadyExists;
        }

        Category category = new(name: command.Name, description: command.Description);
        await _categoryRepository.AddAsync(category);
        var categoryResult = new CreateCategoryResult(category.Id, category.Name, category.Description);

        return categoryResult;
    }
}