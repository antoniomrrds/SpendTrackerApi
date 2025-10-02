namespace SpendTracker.Application.Category.Add;

public interface ICreateCategoryUseCase
{
    Task<bool> Perform(CreateCategoryCommand createCategoryCommand);
}

public sealed record CreateCategoryCommand( string Name, string Description);

public sealed record CreateCategoryResult(Guid Id, string Name, string Description);