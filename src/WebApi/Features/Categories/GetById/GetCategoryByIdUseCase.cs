using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using CategoryDto = WebApi.Features.Categories.Common.CategoryDto;

namespace WebApi.Features.Categories.GetById;

public interface IGetCategoryByIdUseCase
    : IUseCase<GetCategoryByIdInput, Task<Result<CategoryDto?>>>;

internal class GetCategoryByIdUseCase(ICategoryRepository repo) : IGetCategoryByIdUseCase
{
    public async Task<Result<CategoryDto?>> Perform(GetCategoryByIdInput input)
    {
        CategoryDto? result = await repo.GetByIdAsync(input.Id);
        return result is null ? CategoryErrors.NotFound(input.Id.ToString()) : result;
    }
}

public record GetCategoryByIdInput(Guid Id);
