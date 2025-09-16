using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SpendTrackApi.Data;
using SpendTrackApi.Models;

namespace SpendTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Expense : ControllerBase
{
    private readonly AppDbContext _context;


    public Expense(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Category> results = await _context.Categories
            .AsNoTracking()
            .ToListAsync();
        return Ok(results);
    }
}
