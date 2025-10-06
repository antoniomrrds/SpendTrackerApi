using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Helpers;

public  class SqliteInMemoryFixture
{
    public AppDbContext Context { get; }
    public  SqliteInMemoryFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("DataSource=:memory:");
         Context = new AppDbContext(options.Options);
         Context.Database.OpenConnection();
         Context.Database.EnsureCreated();
    }
}