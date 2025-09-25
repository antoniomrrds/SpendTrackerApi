using Microsoft.EntityFrameworkCore;
using SpendTracker.Api.Models;


namespace SpendTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
}
