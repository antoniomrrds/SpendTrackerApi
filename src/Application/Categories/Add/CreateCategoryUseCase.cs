using Application.Abstractions.Data;
using Application.Categories.Common;
using SharedKernel;
using Domain.Categories;

namespace Application.Categories.Add;

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
        Category category = new(name: command.Name, description: command.Description);
        bool categoryAlreadyExists = await _categoryRepository.HasCategoryWithNameAsync(command.Name);
        if (categoryAlreadyExists)
        {
            return CategoryErrors.CategoryNameAlreadyExists;
        }

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();
        
        return new CreateCategoryResult(category.Id, category.Name, category.Description);
    }
}