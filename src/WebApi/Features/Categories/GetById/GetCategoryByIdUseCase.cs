using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using CategoryDto = WebApi.Features.Categories.Common.CategoryDto;

namespace WebApi.Features.Categories.GetById;

public interface IGetCategoryByIdUseCase
    : IUseCase<GetCategoryByIdInput, Task<Result<CategoryDto?>>>;

internal class GetCategoryByIdUseCase(ICategoryReaderRepository repo) : IGetCategoryByIdUseCase
{
    public async Task<Result<CategoryDto?>> Perform(
        GetCategoryByIdInput input,
        CancellationToken cancellationToken = default
    )
    {
        CategoryDto? result = await repo.GetByIdAsync(
            id: input.Id,
            cancellationToken: cancellationToken
        );
        return result is null ? CategoryErrors.NotFound(id: input.Id.ToString()) : result;
    }
}

public record GetCategoryByIdInput(Guid Id);
