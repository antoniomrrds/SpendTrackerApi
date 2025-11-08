using SharedKernel.Abstractions;
using SharedKernel.Abstractions.Data;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Create;

public interface ICreateCategoryUseCase : IUseCase<CreateCategoryInput, Task<Result<CategoryDto>>>;

public sealed record CreateCategoryInput : CommonCategoryProperties;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryWriterRepository _writerRepo;
    private readonly ICategoryCheckRepository _checkRepo;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryUseCase(
        ICategoryWriterRepository writerRepo,
        ICategoryCheckRepository checkRepo,
        IUnitOfWork unitOfWork
    )
    {
        _checkRepo = checkRepo;
        _writerRepo = writerRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CategoryDto>> Perform(CreateCategoryInput input)
    {
        Category category = new(
            id: Guid.NewGuid(),
            name: input.Name,
            description: input.Description
        );
        bool categoryAlreadyExists = await _checkRepo.HasCategoryWithNameAsync(input.Name);
        if (categoryAlreadyExists)
        {
            return CategoryErrors.NameAlreadyExists;
        }

        await _writerRepo.AddAsync(category);
        await _unitOfWork.CommitAsync();

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
        };
    }
}
