using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Data;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Create;

public interface ICreateCategoryUseCase : IUseCase<CreateCategoryInput, Task<Result<CategoryDto>>>;

public sealed record CreateCategoryInput : CommonCategoryProperties;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryDto>> Perform(CreateCategoryInput input)
    {
        Category category = new(name: input.Name, description: input.Description);
        bool categoryAlreadyExists = await _categoryRepository.HasCategoryWithNameAsync(input.Name);
        if (categoryAlreadyExists)
        {
            return CategoryErrors.NameAlreadyExists;
        }

        await _categoryRepository.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }
}
