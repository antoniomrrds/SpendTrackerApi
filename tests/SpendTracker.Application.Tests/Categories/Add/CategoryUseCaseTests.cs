using NSubstitute;
using SpendTracker.Application.Categories.Add;
using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Tests.Categories.Add;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    
    [Fact] public async Task  ShouldInvokeHasCategoryWithNameAsyncOnceAndReturnResult()
    {
        // Arrange
        var name = _faker.Name.FirstName();
        var description = _faker.Commerce.ProductName();
        var command = new CreateCategoryCommand(name, description);

        var categoryRepository = Substitute.For<ICategoryRepository>();
        categoryRepository
            .HasCategoryWithNameAsync(command.Name)
            .Returns(Task.FromResult(false));

        var useCase = new CreateCategoryUseCase(categoryRepository);

        // Act
        var result = await useCase.Perform(command);

        // Assert
        await categoryRepository.Received(1).HasCategoryWithNameAsync(command.Name);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(command.Name);
        result.Value.Description.ShouldBe(command.Description);
    }

    [Fact]
    public async Task ShouldFailWhenCategoryAlreadyExists()
    {
        // Arrange
        var name = _faker.Name.FirstName();
        var description = _faker.Commerce.ProductName();
        var command = new CreateCategoryCommand(name, description);

        var categoryRepository = Substitute.For<ICategoryRepository>();
        categoryRepository
            .HasCategoryWithNameAsync(command.Name)
            .Returns(Task.FromResult(true));

        var useCase = new CreateCategoryUseCase(categoryRepository);

        // Act
        var result = await useCase.Perform(command);
        
        // Assert
        await categoryRepository.Received(1).HasCategoryWithNameAsync(command.Name);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.CategoryNameAlreadyExists);
    }
}