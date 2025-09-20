using FluentValidation;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SpendTrackApi.Data;
using SpendTrackApi.Extensions;
using SpendTrackApi.Models;

using ValidationResult = FluentValidation.Results.ValidationResult;

namespace SpendTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IValidator<CategoryRequest> _validate;

    public CategoryController(AppDbContext context,
        IMapper mapper,
        IValidator<CategoryRequest> validate)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _validate = validate ?? throw new ArgumentNullException(nameof(validate));
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
        ValidationResult result = await _validate.ValidateAsync(request);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        Category category = _mapper.Map<Category>(request);
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

        Category? existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (existingCategory == null)
        {
            return NotFound("Category not found");
        }

        ValidationResult validationResult = await _validate.ValidateAsync(request);
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
        {
            return BadRequest("Id must be greater than 0.");
        }

        Category? existingCategory = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (existingCategory == null)
        {
            return NotFound("Category not found");
        }

        _context.Categories.Remove(existingCategory);

        await _context.SaveChangesAsync();
        return Ok("Category deleted successfully");
    }
}

public class CategoryValidator : AbstractValidator<CategoryRequest>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(4, 150)
            .WithMessage("The name must be between 4 and 150 characters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200).WithMessage("The description cannot exceed 200 characters.")
            .Must(x => string.IsNullOrEmpty(x) || x.Trim().Length > 0)
            .WithMessage("Description cannot be only whitespace.");
    }
}

public record CategoryRequest(
    string Name,
    string Description
);

public record CategoryResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
}
