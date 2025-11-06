using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;
using WebApi.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Common;

public abstract class CategoryValidatorTestBase<TValidator, TInput>
    where TValidator : IValidator<TInput>, new()
    where TInput : CommonCategoryProperties, new()
{
    protected static TValidator Sut => new();
    protected abstract TInput DefaultInput();

    protected TInput BuildCommand(string? name = null, string? description = null)
    {
        TInput input = DefaultInput();

        return input with
        {
            Name = name ?? input.Name,
            Description = description ?? input.Description,
        };
    }

    [Fact]
    public void Validator_WhenNameIsInvalid_ShouldReturnError()
    {
        // Arrange
        TInput input = BuildCommand(name: string.Empty);

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(ValidationMessages.RequiredField.FormatInvariant("Name"));
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnError()
    {
        // Arrange
        TInput input = BuildCommand(name: "i");

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Name", 4, 150)
            );
    }

    [Fact]
    public void Validator_WhenNameIsCorrect_ShouldReturnNameWithoutError()
    {
        // Arrange
        TInput input = BuildCommand();

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        // Arrange
        TInput input = BuildCommand(description: new string('a', 201));

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.MaxChars.FormatInvariant("Description", 200));
    }

    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        // Arrange
        TInput input = BuildCommand(description: "   ");

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.WhiteSpaceOnly.FormatInvariant("Description"));
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotReturnError()
    {
        // Arrange
        TInput input = BuildCommand(description: string.Empty);

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotReturnError()
    {
        // Arrange
        TInput input = BuildCommand();

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}
