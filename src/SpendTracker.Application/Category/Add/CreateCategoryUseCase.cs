using FluentValidation;
namespace SpendTracker.Application.Category.Add;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly IValidator<CreateCategoryCommand> _validator;

    public CreateCategoryUseCase(IValidator<CreateCategoryCommand> createValidator)
    {
        _validator = createValidator;
    }

    public async Task<bool> Perform(CreateCategoryCommand createCategoryCommand)
    {
        await _validator.ValidateAsync(createCategoryCommand);
        return await Task.FromResult(true);
    }
    

}