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

    private readonly Expense _expenseMock = ExpenseMock.GetExpenseFaker();

    [Fact]
    public void Constructor_GivenValidParameters_ThenShouldSetPropertiesCorrectly()
    {
        //Act
        Expense expense = new(_expenseMock.Description, 
            _expenseMock.Amount,
            _expenseMock.Date,
            _expenseMock.CategoryId);
        //Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Description.ShouldBe(_expenseMock.Description);
        expense.Amount.ShouldBe(_expenseMock.Amount);
        expense.Date.ShouldBe(_expenseMock.Date);
        expense.CategoryId.ShouldNotBe(Guid.Empty);
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
        const decimal expectedIncorrectValue = 0;
        const decimal minValue = 0;
        //Arrange 
        Expense expense = new(
            _expenseMock.Description,
            _expenseMock.Amount,
            _expenseMock.Date,
            _expenseMock.CategoryId);

        // Act & Assert constructor
        string expectedMessage = ValidationMessages.GreaterThan.FormatInvariant(nameof(expense.Amount), minValue);
        DomainException ctorException = Should.Throw<DomainException>(() => _ = new Expense(_expenseMock.Description,
            expectedIncorrectValue,
            _expenseMock.Date,
            _expenseMock.CategoryId)
        );
        ctorException.Message.ShouldBe(expectedMessage);
        // Act & Assert setter
        DomainException setterException = Should.Throw<DomainException>(() => expense.SetAmount(expectedIncorrectValue));
        setterException.Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenGreaterThan0_ThenShouldSetAmountCorrectly()
    {
        //Arrange 
        const decimal expectedAmount = 1;
        // Act & Assert constructor
        Expense expense = new(
            _expenseMock.Description,
            _expenseMock.Amount,
            _expenseMock.Date,
            _expenseMock.CategoryId);
        expense.Amount.ShouldBe(_expenseMock.Amount);
        // Act & Assert setter
        expense.SetAmount(expectedAmount); 
        expense.Amount.ShouldBe(expectedAmount);
    }
}