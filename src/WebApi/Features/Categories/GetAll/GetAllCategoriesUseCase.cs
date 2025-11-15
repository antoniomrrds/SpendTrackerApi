using SharedKernel.Abstractions;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetAll;

public interface IGetAllCategoriesUseCase : IUseCaseWithoutInput<Task<IReadOnlyList<CategoryDto>>>;

internal class GetAllCategoriesUseCase(ICategoryReaderRepository repo) : IGetAllCategoriesUseCase
{
    public async Task<IReadOnlyList<CategoryDto>> Perform(
        CancellationToken cancellationToken = default
    ) => await repo.GetAllAsync(cancellationToken: cancellationToken);
}
