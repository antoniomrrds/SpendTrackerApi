using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Helpers;

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
