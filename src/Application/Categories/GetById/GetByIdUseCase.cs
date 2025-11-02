using Application.Categories.Common;
using Domain.Categories;

namespace Application.Categories.GetById;

internal class GetByIdUseCase(ICategoryRepository repo) : IGetByIdUseCase
{
    public async Task<Result<CategoryDto?>> Perform(Guid id)
    {
        CategoryDto? result = await repo.GetByIdAsync(id);
        return result is null ? CategoryErrors.NotFound(id.ToString()) : result;
    }
}
