using Application.Categories.Common;

namespace Application.Categories.Add;

public interface ICreateCategoryUseCase
{
    Task<Result<CategoryDto>> Perform(CreateCategoryCommand command);
}

public sealed record CreateCategoryCommand(string Name, string? Description);
