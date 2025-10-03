using NSubstitute;
using SpendTracker.Application.Categories.Add;
using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Tests.Categories.Add;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly CreateCategoryUseCase _sut;
    public CategoryUseCaseTests()
    { 
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _sut = new CreateCategoryUseCase(_categoryRepositoryMock);
    }

    private CreateCategoryCommand GenerateCommand()
    {
        var name = _faker.Name.FirstName();
        var description = _faker.Commerce.ProductName();
        return new CreateCategoryCommand(name, description);
    }

    [Fact] public async Task  ShouldInvokeHasCategoryWithNameAsyncOnceAndReturnResult_WhenCategoryDoesNotExist()
    {
        // Arrange
        var command = GenerateCommand();

        _categoryRepositoryMock
            .HasCategoryWithNameAsync(command.Name)
            .Returns(Task.FromResult(false));

        // Act
        var result = await _sut.Perform(command);

        // Assert
        await _categoryRepositoryMock.Received(1).HasCategoryWithNameAsync(command.Name);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(command.Name);
        result.Value.Description.ShouldBe(command.Description);
    }

    [Fact]
    public async Task ShouldFailWhenCategoryAlreadyExists()
    {
        // Arrange
        var command = GenerateCommand();
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(command.Name)
            .Returns(Task.FromResult(true));

        // Act
        var result = await _sut.Perform(command);
        
        // Assert
        await _categoryRepositoryMock.Received(1).HasCategoryWithNameAsync(command.Name);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.CategoryNameAlreadyExists);
    }
}