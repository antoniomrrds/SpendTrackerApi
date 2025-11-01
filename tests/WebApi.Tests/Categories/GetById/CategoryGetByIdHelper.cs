using Application.Categories.Common;
using WebApi.Responses.Success;
using WebApi.Tests.Extensions;

namespace WebApi.Tests.Categories.GetById;

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
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        return response;
    }

    public static async Task<CategoryDto> GetCategoryByIdAndReturnDto(
        this HttpClient client,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetCategoryById(id, cancellationToken);
        ApiSuccessResponse<CategoryDto> apiResponse = await response.GetApiResponse<CategoryDto>();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
