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

        var categoryResult = new CreateCategoryResult(Guid.NewGuid(), command.Name, command.Description);

        return categoryResult;
    }
}