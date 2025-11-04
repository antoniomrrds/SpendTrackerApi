using WebApi.Domain.Errors;
using WebApi.Domain.Expenses;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;

namespace WebApi.Tests.Domain.Expenses;

internal static class ExpenseMock
{
    public static Expense CreateFakeExpense()
    {
        Faker<Expense> faker = new("pt_BR");
        return faker
            .CustomInstantiator(f => new Expense(
                description: f.Commerce.ProductName(),
                amount: f.Finance.Amount(10, 5000),
                date: f.Date.Past(30),
                idCategory: f.Random.Guid()
            ))
            .Generate();
    }
}

[Trait("Type", "Unit")]
public class ExpenseTests
{
    private static readonly Expense ExpenseMockInstance = ExpenseMock.CreateFakeExpense();

    private readonly Expense _expenseCorrectlyValues = new(
        ExpenseMockInstance.Description,
        ExpenseMockInstance.Amount,
        ExpenseMockInstance.Date,
        ExpenseMockInstance.IdCategory
    );

    [Fact]
    public void Constructor_GivenValidParameters_ThenShouldSetPropertiesCorrectly()
    {
        _expenseCorrectlyValues.Id.ShouldNotBe(Guid.Empty);
        _expenseCorrectlyValues.Description.ShouldBe(ExpenseMockInstance.Description);
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMockInstance.Amount);
        _expenseCorrectlyValues.Date.ShouldBe(ExpenseMockInstance.Date);
        _expenseCorrectlyValues.IdCategory.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenValueLessThanOrEqualToMin_ThenShouldThrow()
    {
        const decimal expectedIncorrectValue = 0;
        const decimal minValue = 0;

        string expectedMessage = ValidationMessages.GreaterThan.FormatInvariant("Amount", minValue);
        Action callGreaterThan0 = () =>
            _ = new Expense(
                ExpenseMockInstance.Description,
                expectedIncorrectValue,
                ExpenseMockInstance.Date,
                ExpenseMockInstance.IdCategory
            );

        callGreaterThan0.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Action callSetAmount = () => _expenseCorrectlyValues.SetAmount(expectedIncorrectValue);
        callSetAmount.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetAmount_GivenGreaterThan0_ThenShouldSetAmountCorrectly()
    {
        const decimal expectedAmount = 1;
        _expenseCorrectlyValues.Amount.ShouldBe(ExpenseMockInstance.Amount);
        _expenseCorrectlyValues.SetAmount(expectedAmount);
        _expenseCorrectlyValues.Amount.ShouldBe(expectedAmount);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorAndSetDescription_GivenIsEmptyOrNull_ThenShouldThrow(
        string? invalidValues
    )
    {
        string expectedMessage = ValidationMessages.RequiredField.FormatInvariant("Description");
        Action callIsEmptyOrNull = () =>
            _ = new Expense(
                invalidValues!,
                ExpenseMockInstance.Amount,
                ExpenseMockInstance.Date,
                ExpenseMockInstance.IdCategory
            );
        callIsEmptyOrNull.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Action callSetIsNullOrEmpty = () => _expenseCorrectlyValues.SetDescription(invalidValues!);
        callSetIsNullOrEmpty.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void ConstructorAndSetDescription_GivenIsNotNullAndNotEmpty_ThenShouldSetDescriptionCorrectly()
    {
        string expectedDescription = ExpenseMockInstance.Description;
        _expenseCorrectlyValues.Description.ShouldBe(expectedDescription);
        _expenseCorrectlyValues.SetDescription(expectedDescription);
        _expenseCorrectlyValues.Description.ShouldBe(expectedDescription);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(-30)]
    public void ConstructorAndSetDate_GivenDateIsNowOrPast_ThenShouldSetDateCorrectly(
        int daysOffset
    )
    {
        DateTime expectedDate = DateTime.Today.AddDays(daysOffset);
        Expense expense = new(
            ExpenseMockInstance.Description,
            ExpenseMockInstance.Amount,
            expectedDate,
            ExpenseMockInstance.IdCategory
        );
        expense.Date.ShouldBe(expectedDate);
        expense.SetDate(expectedDate);
        expense.Date.ShouldBe(expectedDate);
    }

    [Fact]
    public void ConstructorAndSetDate_GivenDateIsFuture_ThenShouldThrow()
    {
        DateTime futureDate = DateTime.Today.AddDays(1);
        string expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(
            futureDate.ToShortDateString()
        );

        Action callDateIsFuture = () =>
            _ = new Expense(
                ExpenseMockInstance.Description,
                ExpenseMockInstance.Amount,
                futureDate,
                ExpenseMockInstance.IdCategory
            );
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Action callSetDateIsFuture = () => _expenseCorrectlyValues.SetDate(futureDate);
        callSetDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void CheckSealAndPublic_GivenClass_ThenShouldReturnTrue()
    {
        typeof(Expense).ShouldBeSealedAndPublic();
    }

    [Fact]
    public void HasPrivateConstructor_GivenOnePrivateConstructor_ThenShouldReturnTrue()
    {
        typeof(Expense).ShouldHavePrivateConstructor();
    }
}
