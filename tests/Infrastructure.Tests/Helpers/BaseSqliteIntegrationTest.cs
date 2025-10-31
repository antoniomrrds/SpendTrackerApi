using Infrastructure.Persistence.Data;
using TestUtilities.Common;

namespace Infrastructure.Tests.Helpers;

public abstract class BaseSqliteIntegrationTest : TestCommon, IClassFixture<SqliteInMemoryFixture>
{
    protected AppDbContext DbContext { get; init; }

    protected BaseSqliteIntegrationTest(SqliteInMemoryFixture fixture)
    {
        DbContext = fixture.Context;
    }

    public async Task ResetDatabaseAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();
    }
}
