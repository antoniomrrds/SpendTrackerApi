using FluentValidation;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SpendTrackApi.Data;
using SpendTrackApi.Extensions;
using SpendTrackApi.Models;

namespace SpendTrackApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<ExpenseRequest> _validator;

    public ExpenseController(AppDbContext context, IValidator<ExpenseRequest> validator)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseRequest expenseRequest)
    {
        var result = await _validator.ValidateAsync(expenseRequest);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        Expense expenseModel = new()
        {
            Value = expenseRequest.Value,
            CategoryId = expenseRequest.CategoryId,
            Description = expenseRequest.Description,
            Date = expenseRequest.Date ?? DateTime.Now
        };


        await _context.Expenses.AddAsync(expenseModel);
        await _context.SaveChangesAsync();
        return Ok(expenseModel);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
     var categoryResponse = await _context
            .Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(x  => x.Id == id);
        return Ok(categoryResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Expense> results = await _context.Expenses
            .AsNoTracking()
            .ToListAsync();

        return Ok(results);
    }
}

public class ExpenseValidator : AbstractValidator<ExpenseRequest>
{
    private readonly AppDbContext _context;

    public ExpenseValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("The value must be greater than zero.");

        RuleFor(x => x.Description)
            .Empty()
            .WithMessage("The description cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 200 characters.");

        RuleFor(e => e.Date)
            .NotNull().WithMessage("Date cannot be empty.")
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the future.")
            .When(e => e.Date.HasValue);

        RuleFor(e => e.CategoryId)
            .MustAsync(CategoryExists)
            .WithMessage("The specified category does not exist.");
    }

    private async Task<bool> CategoryExists(int categoryId,
        CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == categoryId, cancellationToken);
    }
}

public record ExpenseRequest(
    decimal Value,
    string Description,
    DateTime? Date,
    int CategoryId
);
