using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;
using WebApi.Common.Web.Filters;
using WebApi.Features.Expenses.Common;

namespace WebApi.Features.Expenses.GetById;

public class GetExpenseByIdController(IGetExpenseByIdUseCase useCase) : ExpenseBaseController
{
    [HttpGet("{id}")]
    [ServiceFilter(typeof(ModelBindingEnvelopeFilter))]
    public async Task<IActionResult> GetById([FromRoute] SafeGuid id, CancellationToken ct)
    {
        GetExpenseByIdInput input = new(id);
        Result<ExpenseDto?> result = await useCase.Perform(input, cancellationToken: ct);
        return result.IsSuccess ? Ok(ApiResult.Success(result.Value)) : ToErrorResponse(result);
    }
}
