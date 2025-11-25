using Microsoft.EntityFrameworkCore;
using WebApi.Infrastructure.Persistence.Data;

namespace WebApi.Tests.Infrastructure.Helpers.db;

public class SqliteInMemoryFixture
{
    public AppDbContext Context { get; }

    public SqliteInMemoryFixture()
    {
        DbContextOptionsBuilder<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:")
            .EnableSensitiveDataLogging();
        Context = new AppDbContext(options.Options);
        Context.Database.OpenConnection();
        Context.Database.EnsureCreated();
    }
}
