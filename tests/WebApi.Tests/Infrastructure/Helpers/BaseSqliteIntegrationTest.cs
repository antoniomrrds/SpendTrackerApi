using TestUtilities.Common;
using WebApi.Infrastructure.Persistence.Data;
using WebApi.Tests.Infrastructure.Helpers.db;

namespace WebApi.Tests.Infrastructure.Helpers;

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
