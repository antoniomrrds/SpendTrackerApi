using FluentValidation;
using FluentValidation.Results;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpendTracker.Api.Controllers.Expense;
using SpendTracker.Api.Extensions;
using SpendTracker.Api.Data;

namespace SpendTrackerApi.Controllers.Expense;

[ApiController]
[Route("api/[controller]")]
public sealed class ExpenseController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IValidator<ExpenseRequest> _validator;
    private readonly IMapper _mapper;

    public ExpenseController(AppDbContext context, IValidator<ExpenseRequest> validator, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ExpenseRequest request)
    {
        ValidationResult? result = await _validator.ValidateAsync(request);

        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        Models.Expense expense = _mapper.Map<Models.Expense>(request);
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
        Models.Expense? expenseWithCategory = await _context.Expenses
            .Include(e => e.Category)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == expense.Id);

        if (expenseWithCategory == null)
        {
            return NotFound("Expense not found after creation.");
        }


        ExpenseResponse expenseResponse = _mapper.Map<ExpenseResponse>(expenseWithCategory);
        return CreatedAtAction(nameof(GetById),
            new { id = expense.Id }, expenseResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0.");
        }

        Models.Expense? expenseWithCategory = await _context.Expenses
            .Include(e => e.Category)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);

        if (expenseWithCategory == null)
        {
            return NotFound("Expense not found.");
        }

        ExpenseResponse expenseWithCategoryResponse = _mapper.Map<ExpenseResponse>(expenseWithCategory);

        return Ok(expenseWithCategoryResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        ExpenseRequest request)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0.");
        }

        Models.Expense? existingExpense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
        if (existingExpense is null)
        {
            return NotFound("Expense not found.");
        }

        ValidationResult validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return ValidationProblem(ModelState);
        }

        _mapper.Map(request, existingExpense);
        await _context.SaveChangesAsync();

        Models.Expense? expenseWithCategory = await _context.Expenses
            .Include(e => e.Category)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);

        if (expenseWithCategory == null)
        {
            return NotFound("Expense not found after creation.");
        }

        ExpenseResponse expenseResponse = _mapper.Map<ExpenseResponse>(expenseWithCategory);

        return Ok(expenseResponse);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<Models.Expense> expenseWithCategories = await _context.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .ToListAsync(); 
        
        IEnumerable<ExpenseResponse> expenseResponse = _mapper.Map<IEnumerable<ExpenseResponse>>(expenseWithCategories);

        return Ok(expenseResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (id <= 0)
        {
            return BadRequest("Id must be greater than 0.");
        }

        Models.Expense? existingExpense = await _context.Expenses.FirstOrDefaultAsync(x => x.Id == id);
        if (existingExpense is null)
        {
            return NotFound("Expense not found.");
        }

        _context.Expenses.Remove(existingExpense);
        await _context.SaveChangesAsync();
        return Ok("Expense deleted successfully.");
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotalExpensesAmount()
    {
        decimal totalExpense = await _context.Expenses.SumAsync(x => x.Value);
        string formattedTotalExpense = totalExpense.ToString("C");
        return Ok(formattedTotalExpense);
    }
}