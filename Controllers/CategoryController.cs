using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SpendTrackApi.Data;

namespace SpendTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CategoryController(AppDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<CategoryResponse> categoryResponse = await _context
            .Categories
            .AsNoTracking()
            .ProjectToType<CategoryResponse>(_mapper.Config)
            .ToListAsync();

        return Ok(categoryResponse);
    }
}

public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
