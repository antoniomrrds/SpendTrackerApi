using Application.Categories.Common;
using Domain.Categories;
using Domain.Errors;
using SharedKernel;

namespace Application.Categories.GetById;

internal class GetByIdUseCase : IGetByIdUseCase
{
    private readonly ICategoryRepository _repo;

    public GetByIdUseCase(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<CategoryDto?>> Perform(Guid id)
    {
        if (id == Guid.Empty)
        {
            return CommonErrors.GuidInvalid;
        }

        CategoryDto? result = await _repo.GetByIdAsync(id);
        return result is null ? CategoryErrors.NotFound(id.ToString()) : result;
    }
}
