using WebApi.Domain.Expenses;
using WebApi.Tests.Helpers;

namespace WebApi.Tests.Domain.Expenses;

internal static class ExpenseFixture
{
    private static readonly Faker<Expense> Faker = FakerHelper.CreateFaker<Expense>();

    public static List<Expense> GetExpenses(int count, bool useNewSeed = false) =>
        GetExpenseFaker(useNewSeed).Generate(count);

    public static Expense GetExpense(bool useNewSeed = false) => GetExpenses(1, useNewSeed)[0];

    private static Faker<Expense> GetExpenseFaker(bool useNewSeed)
    {
        int seed = useNewSeed ? SecureSeedGenerator.GetSecureSeed() : 0;
        return Faker
            .CustomInstantiator(f => new Expense(
                id: f.Random.Guid(),
                description: f.Commerce.ProductName(),
                amount: f.Finance.Amount(10, 5000),
                date: f.Date.Past(30),
                idCategory: f.Random.Guid()
            ))
            .UseSeed(seed);
    }
}
