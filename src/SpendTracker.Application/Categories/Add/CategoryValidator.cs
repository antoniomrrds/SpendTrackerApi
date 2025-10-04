using FluentValidation;
using SharedKernel.Resources;

namespace SpendTracker.Application.Categories.Add;

internal class CategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessageProvider.Get(ValidationKeys.RequiredField,
                FieldNameProvider.Get("Name")))
            .Length(4, 150)
            .WithMessage(ValidationMessageProvider.Get(ValidationKeys.StringLengthRangeMessage,
                FieldNameProvider.Get("Name"), 4, 150));

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200).WithMessage(ValidationMessageProvider.Get(ValidationKeys.MaxChars,
                FieldNameProvider.Get("Description"), 200))
            .Must(x => string.IsNullOrEmpty(x) || x.Trim().Length > 0 ) 
            .WithMessage(ValidationMessageProvider.Get(ValidationKeys.WhitespaceOnly, FieldNameProvider.Get("Description")));
    }
}