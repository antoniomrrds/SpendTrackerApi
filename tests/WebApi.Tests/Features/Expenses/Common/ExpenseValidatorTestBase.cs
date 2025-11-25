using FluentValidation;
using FluentValidation.TestHelper;
using SharedKernel.Extensions;
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

    [Fact]
    public void Validator_WhenAmountLessThanOrEqualToMinimum_ShouldReturnError()
    {
        // Arrange
        const decimal minValue = 0;

        TInput input = BuildCommand(c => c.Amount = minValue);

        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Amount)
            .WithErrorMessage(ValidationMessages.GreaterThan.FormatInvariant("Amount", minValue));
    }

    [Fact]
    public void Validator_WhenDateIsFuture_ShouldReturnError()
    {
        //Arrange
        DateTime futureDate = DateTime.Today.AddDays(1);
        TInput input = BuildCommand(c => c.Date = futureDate);
        //Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);
        //Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Date)
            .WithErrorMessage(
                ValidationMessages.DateIsFuture.FormatInvariant(futureDate.ToPtBrDateTime())
            );
    }
}
