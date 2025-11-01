using Application.Categories.Common;

namespace Application.Categories.GetById;

public interface IGetByIdUseCase
{
    Task<Result<CategoryDto?>> Perform(Guid id);
}
