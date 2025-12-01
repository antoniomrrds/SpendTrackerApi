using WebApi.Common.Web.Responses.Success;
using WebApi.Features.Expenses.Common;
using WebApi.Tests.Helpers.Extensions;

namespace WebApi.Tests.Features.Expenses.GetById;

internal static class GetExpenseByIdTestsHelper
{
    public static async Task<HttpResponseMessage> GetExpenseById(
        this HttpClient client,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetAsync(
            ExpenseRoutes.GetById(id),
            cancellationToken
        );
        return response;
    }

    public static async Task<ExpenseDto> GetExpenseByIdAndReturnDto(
        this HttpClient client,
        Guid id,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.GetExpenseById(id, cancellationToken);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<ExpenseDto> apiResponse = await response.GetApiResponse<ExpenseDto>();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
