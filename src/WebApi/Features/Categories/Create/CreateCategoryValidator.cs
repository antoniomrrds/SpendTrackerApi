using FluentValidation;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;

namespace WebApi.Features.Categories.Create;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryInput>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField.FormatInvariant("Name"))
            .Length(4, 150)
            .WithMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Name", 4, 150)
            );

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(200)
            .WithMessage(ValidationMessages.MaxChars.FormatInvariant("Description", 200))
            .Must(x => string.IsNullOrEmpty(x) || x.Trim().Length > 0)
            .WithMessage(ValidationMessages.WhiteSpaceOnly.FormatInvariant("Description"));
    }
}
