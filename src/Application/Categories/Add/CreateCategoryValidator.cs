using Domain.Extensions;
using Domain.Resources;
using FluentValidation;

namespace Application.Categories.Add;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField.FormatInvariant("Name"))
            .Length(4, 150)
            .WithMessage(ValidationMessages.StringLengthRangeMessage.FormatInvariant("Name", 4 , 150));

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200).WithMessage(ValidationMessages.MaxChars.FormatInvariant("Description", 200))
            .Must(x => string.IsNullOrEmpty(x) || x.Trim().Length > 0)
            .WithMessage(ValidationMessages.WhiteSpaceOnly.FormatInvariant("Description"));
    }
}