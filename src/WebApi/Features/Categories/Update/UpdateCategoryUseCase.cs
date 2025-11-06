using SharedKernel.Abstractions;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Update;

public interface IUpdateCategoryUseCase : IUseCase<UpdateCategoryInput, Task<Result<CategoryDto>>>;

public sealed record UpdateCategoryInput : CommonCategoryProperties;

internal class UpdateCategoryUseCase : IUpdateCategoryUseCase
{
    public Task<Result<CategoryDto>> Perform(UpdateCategoryInput input)
    {
        throw new NotImplementedException();
    }
}
