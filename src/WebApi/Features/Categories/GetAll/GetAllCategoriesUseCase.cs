using SharedKernel.Abstractions;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetAll;

public interface IGetAllCategoriesUseCase : IUseCaseWithoutInput<Task<IEnumerable<CategoryDto>>>;

internal class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly ICategoryRepository _repo;

    public GetAllCategoriesUseCase(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<CategoryDto>> Perform()
    {
        return await _repo.GetAllAsync();
    }
}
