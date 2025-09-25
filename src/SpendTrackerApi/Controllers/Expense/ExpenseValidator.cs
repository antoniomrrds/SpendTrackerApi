using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SpendTracker.Api.Data;

namespace SpendTracker.Api.Controllers.Expense;

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
            .NotEmpty()
            .WithMessage("The description cannot be empty.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

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
