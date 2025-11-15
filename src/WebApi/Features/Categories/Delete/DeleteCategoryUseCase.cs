using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Delete;

public interface IDeleteCategoryUseCase : IUseCase<DeleteCategoryInput, Task<Result<bool>>>;

public sealed record DeleteCategoryInput
{
    public Guid Id { get; init; }
}

internal class DeleteCategoryUseCase : IDeleteCategoryUseCase
{
    private readonly ICategoryWriterRepository _writerRepo;

    public DeleteCategoryUseCase(ICategoryWriterRepository writerRepo)
    {
        _writerRepo = writerRepo;
    }

    public async Task<Result<bool>> Perform(
        DeleteCategoryInput input,
        CancellationToken cancellationToken = default
    )
    {
        bool isDeleted = await _writerRepo.DeleteAsync(id: input.Id, cancellationToken);
        if (!isDeleted)
        {
            return CategoryErrors.NotFound(input.Id.ToString());
        }

        return isDeleted;
    }
}
