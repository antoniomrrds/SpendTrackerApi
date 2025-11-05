using WebApi.Common.Web.Responses.Success;
using WebApi.Features.Categories.Common;
using WebApi.Tests.Helpers.Extensions;

namespace WebApi.Tests.Features.Categories.GetAll;

internal static class GetAllCategoriesHelper
{
    public static async Task<HttpResponseMessage> GetAllCategories(
        this HttpClient client,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetAsync(
            CategoriesRoutes.GetAll,
            cancellationToken
        );
        return response;
    }

    public static async Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAndReturnDto(
        this HttpClient client,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetAllCategories(cancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<IReadOnlyList<CategoryDto>> apiResponse = await response.GetApiResponse<
            IReadOnlyList<CategoryDto>
        >();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
