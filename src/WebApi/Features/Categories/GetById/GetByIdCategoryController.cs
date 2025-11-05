using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;
using WebApi.Common.Web.Filters;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.GetById;

public class GetByIdCategoryController : CategoryBaseController
{
    private readonly IGetCategoryByIdUseCase _useCase;

    public GetByIdCategoryController(IGetCategoryByIdUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet("{id}")]
    [ServiceFilter(typeof(ModelBindingEnvelopeFilter))]
    public async Task<IActionResult> GetById([FromRoute] SafeGuid id)
    {
        GetCategoryByIdInput input = new(id);
        Result<CategoryDto?> result = await _useCase.Perform(input);

        return result.IsSuccess
            ? Ok(ApiResult.Success(result.Value))
            : NotFound(ApiResult.NotFound(HttpContext, result.Error.Description));
    }
}
