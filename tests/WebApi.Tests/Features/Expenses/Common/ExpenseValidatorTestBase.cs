using FluentValidation;
using FluentValidation.TestHelper;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;
using WebApi.Features.Expenses.Common;

namespace WebApi.Tests.Features.Expenses.Common;

public abstract class ExpenseValidatorTestBase<TValidator, TInput>
    where TValidator : IValidator<TInput>, new()
    where TInput : CommonExpenseProperties, new()
{
    protected static TValidator Sut => new();
    protected abstract TInput DefaultInput();

    protected TInput BuildCommand(Action<TInput>? configure = null)
    {
        TInput input = DefaultInput();
        configure?.Invoke(input);
        return input;
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void Validator_WhenDescriptionIsInvalid_ShouldReturnError(string? invalidDescription)
    {
        // Arrange
        TInput input = BuildCommand(c => c.Description = invalidDescription!);

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.RequiredField.FormatInvariant("Description"));
    }

    [Theory]
    [InlineData("a")]
    [InlineData("abc")]
    public void Validator_WhenDescriptionIsTooShort_ShouldReturnError(string shortDescription)
    {
        // Arrange
        TInput input = BuildCommand(c => c.Description = shortDescription);

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Description", 4, 500)
            );
    }

    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        //Arrange
        string longDescription = new('a', 501);
        TInput input = BuildCommand(c => c.Description = longDescription);

        //Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        //Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Description", 4, 500)
            );
    }
}
