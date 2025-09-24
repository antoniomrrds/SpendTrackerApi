using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendTrackApi.Data;
using SpendTrackApi.Extensions;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SpendTrackApi.Controllers.Category;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoryRequest> _validator;

    public CategoryController(AppDbContext context,
        IMapper mapper,
        IValidator<CategoryRequest> validate)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validator = validate ?? throw new ArgumentNullException(nameof(validate));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than zero.");
        }

        CategoryResponse? category = await _context.Categories
            .AsNoTracking()
            .ProjectToType<CategoryResponse>(_mapper.Config)
            .FirstOrDefaultAsync(x => x.Id == id);

        return category is null ? NotFound() : Ok(category);
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryRequest request)
    {
        ValidationResult result = await _validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        Models.Category category = _mapper.Map<Models.Category>(request);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        CategoryResponse categoryResponse = _mapper.Map<CategoryResponse>(category);
        return CreatedAtAction(nameof(GetById), new { id = categoryResponse.Id }, categoryResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        [FromBody] CategoryRequest request,
        [FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than zero.");
        }

        Models.Category? existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (existingCategory == null)
        {
            return NotFound("Category not found");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        _mapper.Map(request, existingCategory);

        await _context.SaveChangesAsync();
        CategoryResponse categoryResponse = _mapper.Map<CategoryResponse>(existingCategory);
        return Ok(categoryResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (id <= 0)
            return BadRequest("Id must be greater than 0.");

        int deletedCount = await _context.Categories
            .Where(c => c.Id == id && !_context.Expenses.Any(e => e.CategoryId == c.Id))
            .ExecuteDeleteAsync();

        if (deletedCount != 0)
        {
            return Ok("Category deleted successfully.");
        }

        bool exists = await _context.Categories.AnyAsync(c => c.Id == id);
        return !exists
            ? NotFound("Category not found.")
            : Conflict("Cannot delete categories with associated expenses.");
    }
}