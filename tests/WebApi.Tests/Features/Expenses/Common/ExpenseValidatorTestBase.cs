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

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldReturnError()
    {
        // Arrange
        TInput input = BuildCommand(c => c.Description = string.Empty);
        // Act
        TestValidationResult<TInput>? result = Sut.TestValidate(input);

        // Assert
        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.RequiredField.FormatInvariant("Description"));
    }
}
