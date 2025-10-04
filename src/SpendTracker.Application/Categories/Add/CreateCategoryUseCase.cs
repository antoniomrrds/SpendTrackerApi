using SharedKernel;
using SpendTracker.Application.Abstractions.Data;
using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Categories.Add;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
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
        await _unitOfWork.CommitAsync();
        
        return new CreateCategoryResult(category.Id, category.Name, category.Description);
    }
}