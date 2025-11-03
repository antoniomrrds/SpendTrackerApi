using WebApi.Common.Web.Responses.Success;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Helpers.Extensions;

namespace WebApi.Tests.Features.Categories.GetById;

internal static class CategoryGetHelper
{
    public static async Task<HttpResponseMessage> GetCategoryById(
        this HttpClient client,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetAsync(
            CategoriesRoutes.GetById(id),
            cancellationToken
        );
        return response;
    }

    public static async Task<CategoryDto> GetCategoryByIdAndReturnDto(
        this HttpClient client,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetCategoryById(id, cancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<CategoryDto> apiResponse = await response.GetApiResponse<CategoryDto>();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
