using SharedKernel;

namespace Application.Categories.GetById;

public interface IGetByIdUseCase
{
    Result<bool> Perform(GetByIdCommand  command); 
}

public sealed record GetByIdCommand(Guid Id);