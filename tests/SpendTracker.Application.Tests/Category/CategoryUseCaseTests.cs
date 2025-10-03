using NSubstitute;
using SpendTracker.Application.Category.Add;

namespace SpendTracker.Application.Tests.Category;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;

    private static ICategoryExistsRepository  SetupCategoryExistsRepository(bool exists , string commandName)
    {
        var categoryExistsRepository = Substitute.For<ICategoryExistsRepository>();
        categoryExistsRepository
            .HasCategoryWithNameAsync(commandName)
            .Returns(Task.FromResult(exists));

        return categoryExistsRepository;
    }
    [Fact] public async Task  ShouldInvokeICategoryExistsRepoOnceAndReturnResult()
    {
        // Arrange
        var name = _faker.Name.FirstName();
        var description = _faker.Commerce.ProductName();
        CreateCategoryCommand command = new(name, description);
        var  categoryExistsRepository = SetupCategoryExistsRepository(false,commandName: command.Name);
        CreateCategoryUseCase useCase = new(categoryExistsRepository);

        // Act
        var act = async () => await useCase.Perform(command);
        // Assert
        await act.ShouldNotThrowAsync();
        await categoryExistsRepository.Received(1).HasCategoryWithNameAsync(command.Name);

        var createCategoryResult = await act();
        createCategoryResult.Id.ShouldNotBe(Guid.Empty);
        createCategoryResult.Name.ShouldBe(command.Name);
        createCategoryResult.Description.ShouldBe(command.Description);
    }
}