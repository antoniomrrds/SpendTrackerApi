using Microsoft.EntityFrameworkCore;
using SpendTracker.Api.Models;


namespace SpendTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public required DbSet<Category> Categories { get; set; }
    public required DbSet<Expense> Expenses { get; set; }
}