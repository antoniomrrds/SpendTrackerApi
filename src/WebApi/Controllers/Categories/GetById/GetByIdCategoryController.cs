using Application.Categories.Common;
using Application.Categories.GetById;
using WebApi.Controllers.Common;
using WebApi.Factories;
using WebApi.Filters;

namespace WebApi.Controllers.Categories.GetById;

public class GetByIdCategoryController : CategoriesBaseController
{
    private readonly IGetByIdUseCase _useCase;

    public GetByIdCategoryController(IGetByIdUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{id}")]
    [ServiceFilter(typeof(ModelBindingEnvelopeFilter))]
    public async Task<IActionResult> GetById([FromRoute] SafeGuid id)
    {
        Result<CategoryDto?> result = await _useCase.Perform(id);

        return result.IsSuccess
            ? Ok(ApiResult.Success(result.Value))
            : NotFound(ApiResult.NotFound(HttpContext, result.Error.Description));
    }
}
