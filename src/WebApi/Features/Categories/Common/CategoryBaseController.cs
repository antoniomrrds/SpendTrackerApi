using WebApi.Common.Web.Controllers;
using WebApi.Common.Web.Factories;
using WebApi.Domain.Categories;

namespace WebApi.Features.Categories.Common;

[Route("api/categories")]
public class CategoryBaseController : BaseApiController
{
    protected override IActionResult ToErrorResponse(Result result)
    {
        return result.Error.Code switch
        {
            CategoryErrorCodes.NameAlreadyExists => Conflict(
                ApiResult.Conflict(HttpContext, result.Error.Description)
            ),
            _ => NotFound(ApiResult.NotFound(HttpContext, result.Error.Description)),
        };
    }
}
