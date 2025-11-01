using Application.Categories.Common;
using Application.Categories.GetById;
using WebApi.Extensions;

namespace WebApi.Controllers.Categories.GetById;

public class GetByIdCategoryController(IGetByIdUseCase useCase) : CategoriesBaseController
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        Result<CategoryDto?> result = await useCase.Perform(id);

        return result.IsFailure
            ? this.ToNotFoundProblem(result.Error.Description)
            : this.ToOkResponse(result.Value);
    }
}
