using SpendTracker.Domain.Entities;
using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Tests.Entities;

internal static class ExpenseMock
{
    public static Expense CreateFakeExpense()
    {
        Faker<Expense> faker = new("pt_BR");
        return faker
            .CustomInstantiator(f => new Expense(
                description: f.Commerce.ProductName(),
                amount: f.Finance.Amount(10, 5000),
                date: f.Date.Recent(30),
                categoryId: f.Random.Guid()
            ))
            .Generate();
    }
}

public class ExpenseTests
{
    // serve para printar e outras coisas no tests
    // private readonly ITestOutputHelper _output;
    // public ExpenseTests(ITestOutputHelper output)
    // {
    //     _output = output;
    // }

    private static readonly Expense ExpenseMockInstance = ExpenseMock.CreateFakeExpense();
    private readonly Expense _expenseCorrectlyValues = new(
        ExpenseMockInstance.Description,
        ExpenseMockInstance.Amount,
        ExpenseMockInstance.Date,
        ExpenseMockInstance.CategoryId
    );

    [Fact]
    public void Constructor_GivenValidParameters_ThenShouldSetPropertiesCorrectly()
    {
        _expenseCorrectlyValues.Id.ShouldNotBe(Guid.Empty);
        _expenseCorrectlyValues.Description.ShouldBe(ExpenseMockInstance.Description);
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMockInstance.Amount);
        _expenseCorrectlyValues.Date.ShouldBe(ExpenseMockInstance.Date);
        _expenseCorrectlyValues.CategoryId.ShouldNotBe(Guid.Empty);
    }
    [Fact]
    public void ConstructorAndSetAmount_GivenValueLessThanOrEqualToMin_ThenShouldThrow()
    {
        //Arrange
        const decimal expectedIncorrectValue = 0;
        const decimal minValue = 0;

        // Act & Assert constructor
        var expectedMessage =
            ValidationMessages.GreaterThan.FormatInvariant(nameof(_expenseCorrectlyValues.Amount), minValue);
        Action callGreaterThan0 = () => _ = new Expense(ExpenseMockInstance.Description,
                expectedIncorrectValue,
                ExpenseMockInstance.Date,
                ExpenseMockInstance.CategoryId);

        callGreaterThan0.ShouldThrowWithMessage<DomainException>(expectedMessage);

        var callSetAmount = () => _expenseCorrectlyValues.SetAmount(expectedIncorrectValue);
        // Act & Assert setter
        callSetAmount.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenGreaterThan0_ThenShouldSetAmountCorrectly()
    {
        //Arrange 
        const decimal expectedAmount = 1;
        // Act & Assert constructor
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMockInstance.Amount);
        // Act & Assert setter
        _expenseCorrectlyValues.SetAmount(expectedAmount);
        _expenseCorrectlyValues.Amount.ShouldBe(expectedAmount);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorAndSetDescription_GivenIsEmptyOrNull_ThenShouldThrow(string? invalidValues)
    {
        var expectedMessage =
            ValidationMessages.RequiredField.FormatInvariant(nameof(_expenseCorrectlyValues.Description));
        // Act & Assert constructor
        Action callIsEmptyOrNull = () => _ = new Expense(invalidValues!,
            ExpenseMockInstance.Amount,
            ExpenseMockInstance.Date,
            ExpenseMockInstance.CategoryId);
        callIsEmptyOrNull.ShouldThrowWithMessage<DomainException>(expectedMessage);

        // Act & Assert setter
        var callSetIsNullOrEmpty = () => _expenseCorrectlyValues.SetDescription(invalidValues!);
        callSetIsNullOrEmpty.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetDescription_GivenIsNotNullAndNotEmpty_ThenShouldSetDescriptionCorrectly()
    {
        //Arrange 
        var expectedDescription = ExpenseMockInstance.Description;
        // Act & Assert constructor
        _expenseCorrectlyValues.Description.ShouldBe(expectedDescription);
        // Act & Assert setter
        _expenseCorrectlyValues.SetDescription(expectedDescription);
        _expenseCorrectlyValues.Description.ShouldBe(expectedDescription);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(-30)]
    public void ConstructorAndSetDate_GivenDateIsNowOrPast_ThenShouldSetDateCorrectly(int daysOffset)
    {
        var expectedDate = DateTime.Today.AddDays(daysOffset);
        // Act & Assert constructor
        Expense expense = new(ExpenseMockInstance.Description, ExpenseMockInstance.Amount, expectedDate, ExpenseMockInstance.CategoryId);
        expense.Date.ShouldBe(expectedDate);
        // Act & Assert setter
        expense.SetDate(expectedDate);
        expense.Date.ShouldBe(expectedDate);
    }

    [Fact]
    public void ConstructorAndSetDate_GivenDateIsFuture_ThenShouldThrow()
    {
        var expectedDate = DateTime.Today.AddDays(1);

        // Act & Assert constructor
        var expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(expectedDate);

        Action callDateIsFuture = () => _ = new Expense(ExpenseMockInstance.Description,
            ExpenseMockInstance.Amount,
            expectedDate,
            ExpenseMockInstance.CategoryId);
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);

        // Act & Assert setter
        var callSetDateIsFuture = () => _expenseCorrectlyValues.SetDate(expectedDate);
        callSetDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void CheckSealAndPublic_GivenClass_ThenShouldReturnTrue()
    {
        //Assert
        typeof(Expense).ShouldBeSealedAndPublic();
    }

    [Fact]
    public void HasPrivateConstructor_GivenOnePrivateConstructor_ThenShouldReturnTrue()
    {
        //Assert
        typeof(Expense).ShouldHavePrivateConstructor();
    }
}