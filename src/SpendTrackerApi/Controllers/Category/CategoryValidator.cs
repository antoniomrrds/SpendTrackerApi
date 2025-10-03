using FluentValidation;

namespace SpendTracker.Api.Controllers.Category;

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