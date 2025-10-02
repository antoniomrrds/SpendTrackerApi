namespace SpendTracker.Application.Category.Add;

public interface ICreateCategoryUseCase
{
    Task Perform(CreateCategoryCommand command);
}

public sealed record CreateCategoryCommand( string Name, string Description);

public sealed record CreateCategoryResult(Guid Id, string Name, string Description);