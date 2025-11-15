using WebApi.Common.Web.Factories;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetAll;

public class GetAllCategoriesController(IGetAllCategoriesUseCase useCase) : CategoryBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        IReadOnlyList<CategoryDto> result = await useCase.Perform(ct);
        return Ok(ApiResult.Success(result));
    }
}
