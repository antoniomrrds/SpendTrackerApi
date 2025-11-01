using Application.Categories.Common;
using Domain.Categories;
using Domain.Errors;

namespace Application.Categories.GetById;

internal class GetByIdUseCase(ICategoryRepository repo) : IGetByIdUseCase
{
    public async Task<Result<CategoryDto?>> Perform(Guid id)
    {
        if (id == Guid.Empty)
        {
            return CommonErrors.GuidInvalid;
        }

        CategoryDto? result = await repo.GetByIdAsync(id);
        return result is null ? CategoryErrors.NotFound(id.ToString()) : result;
    }
}
