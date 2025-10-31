using SharedKernel;

namespace Application.Categories.Add;

public interface ICreateCategoryUseCase
{
    Task<Result<CreateCategoryResult>> Perform(CreateCategoryCommand command);
}

public sealed record CreateCategoryCommand(string Name, string? Description);

public sealed record CreateCategoryResult(Guid Id, string Name, string? Description);
