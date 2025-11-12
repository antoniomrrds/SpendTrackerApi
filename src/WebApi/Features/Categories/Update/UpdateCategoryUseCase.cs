using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Update;

public interface IUpdateCategoryUseCase : IUseCase<UpdateCategoryInput, Task<Result<CategoryDto>>>;

public sealed record UpdateCategoryInput : CommonCategoryProperties
{
    public Guid Id { get; init; }
}

internal class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    private readonly ICategoryWriterRepository _writerRepository;
    private readonly ICategoryCheckRepository _checkRepository;
    private readonly ICategoryReaderRepository _readerRepository;

    public UpdateCategoryUseCase(
        ICategoryWriterRepository writerRepository,
        ICategoryCheckRepository checkRepository,
        ICategoryReaderRepository readerRepository
    )
    {
        _writerRepository = writerRepository;
        _checkRepository = checkRepository;
        _readerRepository = readerRepository;
    }

    public async Task<Result<CategoryDto>> Perform(UpdateCategoryInput input)
    {
        Category category = new(id: input.Id, name: input.Name, description: input.Description);

        bool nameAlreadyTaken = await _checkRepository.HasCategoryWithNameAsync(
            category.Name,
            excludeId: category.Id
        );

        if (nameAlreadyTaken)
        {
            return CategoryErrors.NameAlreadyExists;
        }

        bool isUpdated = await _writerRepository.UpdateAsync(category);
        if (!isUpdated)
        {
            return CategoryErrors.NotFound(category.Id.ToString());
        }

        CategoryDto? updated = await _readerRepository.GetByIdAsync(category.Id);
        return updated is null ? CategoryErrors.NotFound(category.Id.ToString()) : updated;
    }
}
