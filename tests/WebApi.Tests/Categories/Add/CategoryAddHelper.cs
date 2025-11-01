using Application.Categories.Common;
using WebApi.Controllers.Categories.Add;
using WebApi.Responses.Success;
using WebApi.Tests.Extensions;

namespace WebApi.Tests.Categories.Add;

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
