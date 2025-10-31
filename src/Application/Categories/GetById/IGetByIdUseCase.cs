using Application.Categories.Common;
using SharedKernel;

namespace Application.Categories.GetById;

public interface IGetByIdUseCase
{
    Task<Result<CategoryDto?>> Perform(Guid id);
}
