using FluentValidation;
using FluentValidation.Results;

namespace SpendTracker.Application.Category.Add;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryUseCase(IValidator<CreateCategoryCommand> createValidator)
    {
        _validator = createValidator;
    }

    public async Task<bool> Perform(CreateCategoryCommand command)
    {
        await _validator.ValidateAsync(command);

        return await Task.FromResult(true);
    }
}