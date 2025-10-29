using Domain.Categories;
using SharedKernel;

namespace Application.Categories.GetById;

public interface IGetByIdUseCase
{
    Task<Result<Category?>> Perform(Guid  id); 
}
