using FluentValidation;
using NSubstitute;
using SpendTracker.Application.Category.Add;

namespace SpendTracker.Application.Tests.Category;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    
    [Fact]
    [Trait("Perform", "CategoryUseCaseTests")]
    public async Task Perform_ShouldCallValidator_WhenCommandIsValid()
    {
        // Arrange
        string? name = _faker.Name.FirstName();
        string? description = _faker.Commerce.ProductName();
        CreateCategoryCommand command = new(name, description);

        IValidator<CreateCategoryCommand>? validator = Substitute.For<IValidator<CreateCategoryCommand>>();

        await validator
            .ValidateAsync(command, Arg.Any<CancellationToken>());

        CreateCategoryUseCase useCase = new(validator);

        // Act
        Func<Task<bool>> act = async () => await useCase.Perform(command);

        // Assert
        await act.ShouldNotThrowAsync();
        await validator.Received(1).ValidateAsync(command, Arg.Any<CancellationToken>());
    }

}