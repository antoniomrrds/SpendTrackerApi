using SpendTracker.Domain.Entities;
using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Tests.Entities;

internal static class ExpenseMock
{
    public static Expense GetExpenseFaker() => new Faker<Expense>()
        .CustomInstantiator(_ => new Expense())
        .RuleFor(e => e.Id, f => f.Random.Guid())
        .RuleFor(e => e.Description, f => f.Commerce.ProductName())
        .RuleFor(e => e.Amount, f => f.Finance.Amount(10, 5000))
        .RuleFor(e => e.Date, f => f.Date.Recent(30))
        .RuleFor(e => e.CategoryId, f => f.Random.Guid())
        .Generate();
}

public class ExpenseTests
{
    // serve para printar e outras coisas no tests
    // private readonly ITestOutputHelper _output;
    // public ExpenseTests(ITestOutputHelper output)
    // {
    //     _output = output;
    // }

    private static readonly Expense ExpenseMock = Entities.ExpenseMock.GetExpenseFaker();

    private readonly Expense _expenseCorrectlyValues = new(
        ExpenseMock.Description,
        ExpenseMock.Amount,
        ExpenseMock.Date,
        ExpenseMock.CategoryId);

    [Fact]
    public void Constructor_GivenValidParameters_ThenShouldSetPropertiesCorrectly()
    {
        _expenseCorrectlyValues.Id.ShouldNotBe(Guid.Empty);
        _expenseCorrectlyValues.Description.ShouldBe(ExpenseMock.Description);
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMock.Amount);
        _expenseCorrectlyValues.Date.ShouldBe(ExpenseMock.Date);
        _expenseCorrectlyValues.CategoryId.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Constructor_GivenParameterless_ThenShouldCreateObjectWithDefaultValues()
    {
        //Act
        Expense expense = new();

        //Assert
        expense.Id.ShouldBe(Guid.Empty);
        expense.Description.ShouldBe(string.Empty);
        expense.Amount.ShouldBe(0m);
        expense.Date.ShouldBe(default);
        expense.CategoryId.ShouldBe(Guid.Empty);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenValueLessThanOrEqualToMin_ThenShouldThrow()
    {
        //Arrange
        const decimal expectedIncorrectValue = 0;
        const decimal minValue = 0;

        // Act & Assert constructor
        string expectedMessage =
            ValidationMessages.GreaterThan.FormatInvariant(nameof(_expenseCorrectlyValues.Amount), minValue);
        Action callGreaterThan0 = () => _ = new Expense(ExpenseMock.Description,
                expectedIncorrectValue,
                ExpenseMock.Date,
                ExpenseMock.CategoryId);

        callGreaterThan0.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Action callSetAmount = () => _expenseCorrectlyValues.SetAmount(expectedIncorrectValue);
        // Act & Assert setter
        callSetAmount.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenGreaterThan0_ThenShouldSetAmountCorrectly()
    {
        //Arrange 
        const decimal expectedAmount = 1;
        // Act & Assert constructor
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMock.Amount);
        // Act & Assert setter
        _expenseCorrectlyValues.SetAmount(expectedAmount);
        _expenseCorrectlyValues.Amount.ShouldBe(expectedAmount);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorAndSetDescription_GivenIsEmptyOrNull_ThenShouldThrow(string? invalidValues)
    {
        string expectedMessage =
            ValidationMessages.RequiredField.FormatInvariant(nameof(_expenseCorrectlyValues.Description));
        // Act & Assert constructor
        Action callIsEmptyOrNull = () => _ = new Expense(invalidValues!,
            ExpenseMock.Amount,
            ExpenseMock.Date,
            ExpenseMock.CategoryId);
        callIsEmptyOrNull.ShouldThrowWithMessage<DomainException>(expectedMessage);

        // Act & Assert setter
        Action callSetIsNullOrEmpty = () => _expenseCorrectlyValues.SetDescription(invalidValues!); 
        callSetIsNullOrEmpty.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetDescription_GivenIsNotNullAndNotEmpty_ThenShouldSetDescriptionCorrectly()
    {
        //Arrange 
        string expectedDescription = ExpenseMock.Description;
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
        DateTime expectedDate = DateTime.Today.AddDays(daysOffset);
        // Act & Assert constructor
        Expense expense = new(ExpenseMock.Description, ExpenseMock.Amount, expectedDate, ExpenseMock.CategoryId);

        expense.Date.ShouldBe(expectedDate);
        // Act & Assert setter
        expense.SetDate(expectedDate);
        expense.Date.ShouldBe(expectedDate);
    }

    [Fact]
    public void ConstructorAndSetDate_GivenDateIsFuture_ThenShouldThrow()
    {
        DateTime expectedDate = DateTime.Today.AddDays(1);

        // Act & Assert constructor
        string expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(expectedDate);

        Action callDateIsFuture = () => _ = new Expense(ExpenseMock.Description,
            ExpenseMock.Amount,
            expectedDate,
            ExpenseMock.CategoryId);
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
        
        // Act & Assert setter
        Action callSetDateIsFuture = () => _expenseCorrectlyValues.SetDate(expectedDate);
        callSetDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }
}