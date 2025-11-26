using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Filters;
using WebApi.Features.Categories.Common;

namespace WebApi.Features.Categories.Delete;

public class DeleteCategoryController : CategoryBaseController
{
    private readonly IDeleteCategoryUseCase _useCase;

    public DeleteCategoryController(IDeleteCategoryUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ModelBindingEnvelopeFilter))]
    public async Task<IActionResult> Delete([FromRoute] SafeGuid id, CancellationToken ct)
    {
        DeleteCategoryInput input = new() { Id = id };
        Result<bool> result = await _useCase.Perform(input: input, cancellationToken: ct);
        return result.IsSuccess ? NoContent() : ToErrorResponse(result);
    }
}
