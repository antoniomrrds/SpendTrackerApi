using WebApi.Domain.Expenses;
using WebApi.Tests.Domain.Expenses;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Helpers.db;

namespace WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

public abstract class ExpenseIntegrationTestBase : BaseSqliteIntegrationTest, IAsyncLifetime
{
    protected Expense Expense { get; set; }
    protected IEnumerable<Expense> Expenses { get; set; }

    protected ExpenseIntegrationTestBase(SqliteInMemoryFixture fixture)
        : base(fixture)
    {
        Expense = ExpenseFixture.GetExpense(true);
        Expenses = ExpenseFixture.GetExpenses(3, true);
    }

    public async ValueTask InitializeAsync()
    {
        await ResetDatabaseAsync();
        DbContext.ChangeTracker.Clear();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    protected async Task<Expense> MakeCreateExpenseAsync()
    {
        await ExpenseSeeder.AddAsync(DbContext, Expense);
        return Expense;
    }
}
