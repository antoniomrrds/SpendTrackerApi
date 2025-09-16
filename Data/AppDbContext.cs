using Microsoft.EntityFrameworkCore;

using SpendTrackApi.Models;

namespace SpendTrackApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}
