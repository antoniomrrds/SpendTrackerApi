using WebApi.Features.Expenses.Common;
using WebApi.Infrastructure.Persistence.Repositories.Expenses;
using WebApi.Tests.Infrastructure.Helpers.db;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

[Trait("Type", "Integration")]
public class ExpenseReaderRepositoryTests : ExpenseIntegrationTestBase
{
    private readonly ExpenseReaderRepository _sut;

    public ExpenseReaderRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new ExpenseReaderRepository(DbContext);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExpenseExists_ShouldReturnExpenseDto()
    {
        //Arrange
        await MakeCreateExpenseAsync();
        //Act
        ExpenseDto? result = await _sut.GetByIdAsync(Expense.Id, Ct);

        //Assert
        result.ShouldNotBeNull();
        result.ShouldSatisfyAllConditions(
            r => r.Id.ShouldBe(Expense.Id),
            r => r.Description.ShouldBe(Expense.Description),
            r => r.Amount.ShouldBe(Expense.Amount),
            r => r.Date.ShouldBe(Expense.Date),
            r => r.CategoryId.ShouldBe(Expense.IdCategory),
            r => r.CategoryName.ShouldBe(Category.Name),
            r => r.DateFormatted.ShouldNotBeNullOrEmpty(),
            r => r.AmountFormatted.ShouldNotBeNullOrEmpty()
        );
    }

    [Fact]
    public async Task GetByIdAsync_WhenExpenseDoesNotExist_ShouldReturnNull()
    {
        //Act
        ExpenseDto? result = await _sut.GetByIdAsync(Expense.Id, Ct);

        //Assert
        result.ShouldBeNull();
    }
}
