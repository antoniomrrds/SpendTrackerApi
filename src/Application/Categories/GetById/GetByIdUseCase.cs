using Domain.Errors;
using SharedKernel;

namespace Application.Categories.GetById;

internal class GetByIdUseCase:IGetByIdUseCase
{
    public Result<bool> Perform(GetByIdCommand command)
    {
            if (command.Id == Guid.Empty)
            {
                return CommonErrors.GuidInvalid;
            }
           
            return false;
    }
}