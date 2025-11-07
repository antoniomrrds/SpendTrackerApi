using WebApi.Common.Web.Responses.Success;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;

namespace WebApi.Tests.Features.Categories.Add;

internal static class CategoryAddHelper
{
    public static async Task<HttpResponseMessage> AddCategory(
        this HttpClient client,
        CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(
            CategoriesRoutes.Add,
            request,
            cancellationToken
        );
        return response;
    }

    public static async Task<CategoryDto> AddCategoryAndReturnDto(
        this HttpClient client,
        CreateCategoryRequest request,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.AddCategory(request, cancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<CategoryDto> apiResponse = await response.GetApiResponse<CategoryDto>();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
