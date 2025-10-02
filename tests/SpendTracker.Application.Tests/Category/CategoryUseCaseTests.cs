using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using SpendTracker.Application.Category.Add;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SpendTracker.Application.Tests.Category;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    private static readonly CancellationToken CancellationToken = Arg.Any<CancellationToken>();
 
    private static IValidator<CreateCategoryCommand> SetupValidator(ValidationResult result)
    {
        IValidator<CreateCategoryCommand>? validator = Substitute.For<IValidator<CreateCategoryCommand>>();
        validator.ValidateAsync(Arg.Any<CreateCategoryCommand>(), CancellationToken)
            .Returns(Task.FromResult(result));
        return validator;
    }
    
    [Fact]
    public async Task ShouldCallValidatorWhenCommandIsValid()
    {
        // Arrange
        string? name = _faker.Name.FirstName();
        string? description = _faker.Commerce.ProductName();
        CreateCategoryCommand command = new(name, description);

        IValidator<CreateCategoryCommand> validator = SetupValidator(new ValidationResult());
        CreateCategoryUseCase useCase = new(validator);
        // Act
        Func<Task<bool>> act = async () => await useCase.Perform(command);
        // Assert
        await act.ShouldNotThrowAsync();
        await validator.Received(1).ValidateAsync(command, CancellationToken);
    }

    [Fact]
    public async Task ShouldThrowWhenValidationFails()
    {
        CreateCategoryCommand command = new("", _faker.Commerce.ProductName());
        ValidationResult result = new([
            new ValidationFailure("Name", "Name is required")
        ]);

        IValidator<CreateCategoryCommand> validator = SetupValidator(result);
        CreateCategoryUseCase useCase = new(validator);

        Func<Task<bool>> act = async () => await useCase.Perform(command);

        ValidationException exception = await act.ShouldThrowAsync<ValidationException>();
        exception.Errors.ShouldContain(e => e.PropertyName == "Name" && e.ErrorMessage == "Name is required");
        
    }

}