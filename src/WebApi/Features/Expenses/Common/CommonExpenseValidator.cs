using FluentValidation;
using SharedKernel.Extensions;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;

namespace WebApi.Features.Expenses.Common;

public abstract class CommonExpenseValidator<T> : AbstractValidator<T>
    where T : CommonExpenseProperties
{
    protected CommonExpenseValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage(ValidationMessages.RequiredField.FormatInvariant("Description"))
            .Length(4, 500)
            .WithMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Description", 4, 500)
            );

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GreaterThan.FormatInvariant("Amount", 0));

        RuleFor(e => e.Date)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage(e =>
                ValidationMessages.DateIsFuture.FormatInvariant(e.Date.ToPtBrDateTime())
            );
    }
}
