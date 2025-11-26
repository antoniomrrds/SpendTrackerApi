using Microsoft.EntityFrameworkCore;
using WebApi.Domain.Expenses;
using WebApi.Infrastructure.Persistence.Repositories.Expenses;
using WebApi.Tests.Infrastructure.Helpers.db;
using WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

[Trait("Type", "Integration")]
public class ExpenseWriterRepositoryTests : ExpenseIntegrationTestBase
{
    private readonly ExpenseWriterRepository _sut;

    public ExpenseWriterRepositoryTests(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        _sut = new ExpenseWriterRepository(DbContext);
    }

    [Fact]
    public async Task AddAsync_WhenExpenseIsValid_ShouldPersistExpense()
    {
        //Arrange
        await CategorySeeder.AddAsync(DbContext, Category);
        Expense.IdCategory = Category.Id;
        await _sut.AddAsync(Expense, Ct);

        await DbContext.SaveChangesAsync(Ct);
        //Act
        Expense? saved = await DbContext.Expenses.FirstOrDefaultAsync(c => c.Id == Expense.Id, Ct);
        //Assert
        saved.ShouldNotBeNull();
        saved.Id.ShouldBe(Expense.Id);
        saved.Amount.ShouldBe(Expense.Amount);
        saved.Description.ShouldBe(Expense.Description);
        saved.IdCategory.ShouldBe(Expense.IdCategory);
        saved.Date.ShouldBe(Expense.Date);
    }
}
