using FluentValidation.TestHelper;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Features.Expenses.Common;

namespace WebApi.Tests.Features.Expenses.Create;

[Trait("Type", "Unit")]
public class CreateExpenseValidatorTests
    : ExpenseValidatorTestBase<CreateExpenseValidator, CreateExpenseInput>
{
    private readonly CreateExpenseInput _createExpenseInput = CreateExpenseFixture.ExpenseInput();

    protected override CreateExpenseInput DefaultInput() =>
        new()
        {
            Description = _createExpenseInput.Description,
            Amount = _createExpenseInput.Amount,
            Date = _createExpenseInput.Date,
            IdCategory = _createExpenseInput.IdCategory,
        };

    [Fact]
    public void Validator_WhenInputIsFullyValid_ShouldPassAllValidations()
    {
        // Arrange
        CreateExpenseInput input = DefaultInput();
        // Act
        TestValidationResult<CreateExpenseInput>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
