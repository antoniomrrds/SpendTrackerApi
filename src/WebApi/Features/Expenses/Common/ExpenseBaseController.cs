using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;

namespace WebApi.Features.Expenses.Common;

[Route("api/expenses")]
public class ExpenseBaseController : BaseApiController
{
    protected override IActionResult ToErrorResponse(Result result)
    {
        return result.Error.Code switch
        {
            _ => NotFound(ApiResult.NotFound(HttpContext, result.Error.Description)),
        };
    }
}
