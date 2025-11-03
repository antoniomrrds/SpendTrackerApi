using SharedKernel.Abstractions;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetAll;

public interface IGetAllCategoriesUseCase : IUseCaseWithoutInput<Task<IReadOnlyList<CategoryDto>>>;

internal class GetAllCategoriesUseCase(ICategoryRepository repo) : IGetAllCategoriesUseCase
{
    public async Task<IReadOnlyList<CategoryDto>> Perform() => [.. await repo.GetAllAsync()];
}
