using SharedKernel.Abstractions;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Expenses.Create;

public interface ICreateExpenseUseCase : IUseCase<CreateExpenseInput, Task<Result<bool>>>;

public record CreateExpenseInput
{
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public Guid IdCategory { get; init; }
}

internal class CreateExpenseUseCase : ICreateExpenseUseCase
{
    private readonly ICategoryCheckRepository _categoryCheckRepo;

    public CreateExpenseUseCase(ICategoryCheckRepository categoryCheckRepo)
    {
        _categoryCheckRepo = categoryCheckRepo;
    }

    public async Task<Result<bool>> Perform(
        CreateExpenseInput input,
        CancellationToken cancellationToken = default
    )
    {
        bool categoryExists = await _categoryCheckRepo.CategoryExists(
            input.IdCategory,
            cancellationToken
        );
        if (categoryExists)
        {
            return CategoryErrors.NotFound(input.IdCategory.ToString());
        }

        return true;
    }
}
