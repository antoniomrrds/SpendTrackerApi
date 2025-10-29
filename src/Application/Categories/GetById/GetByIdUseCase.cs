using Application.Categories.Common;
using Domain.Categories;
using Domain.Errors;
using SharedKernel;

namespace Application.Categories.GetById;

internal class GetByIdUseCase:IGetByIdUseCase
{
    private readonly ICategoryRepository _repo;

    public GetByIdUseCase(ICategoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<Category?>> Perform(Guid id)
    {
            if (id == Guid.Empty)
            {
                return CommonErrors.GuidInvalid;
            }

            Category? result =   await _repo.GetByIdAsync(id);
            if (result is null)
            {
                return CategoryErrors.NotFound(id.ToString());
            }

            return result;
    }
    
}