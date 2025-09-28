using SpendTracker.Domain.Entities;

namespace SpendTracker.Domain.Tests.Entities;

internal static class ExpenseMock
{
    public static Expense GetExpenseFaker() => new Faker<Expense>()
        .CustomInstantiator(_ => new Expense())
        .RuleFor(e => e.Id, f => f.Random.Guid())
        .RuleFor(e => e.Description, f => f.Commerce.ProductName())
        .RuleFor(e => e.Value, f => f.Finance.Amount(10,5000))
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
            _expenseMock.Value,
            _expenseMock.Date,
            _expenseMock.CategoryId);
        //Assert
        expense.Id.ShouldNotBe(Guid.Empty);
        expense.Description.ShouldBe(_expenseMock.Description);
        expense.Value.ShouldBe(_expenseMock.Value);
        expense.Date.ShouldBe(_expenseMock.Date);
        expense.CategoryId.ShouldNotBe(Guid.Empty);
    }
}