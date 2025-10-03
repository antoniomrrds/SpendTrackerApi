namespace SpendTracker.Application.Category.Add;

internal class CreateCategoryUseCase : ICreateCategoryUseCase
{
    private readonly ICategoryExistsRepository _categoryExistsRepository;
    public CreateCategoryUseCase(ICategoryExistsRepository categoryExistsRepository)
    {
        _categoryExistsRepository = categoryExistsRepository;
    }

    public async Task<CreateCategoryResult> Perform(CreateCategoryCommand command)
    {

    }
}