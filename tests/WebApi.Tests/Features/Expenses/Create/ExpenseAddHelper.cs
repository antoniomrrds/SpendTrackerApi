using WebApi.Common.Web.Responses.Success;
using WebApi.Features.Expenses.Common;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Helpers.Extensions;

namespace WebApi.Tests.Features.Expenses.Create;

public static class ExpenseAddHelper
{
    public static async Task<HttpResponseMessage> AddExpense(
        this HttpClient client,
        CreateExpenseRequest request,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.PostAsJsonAsync(
            ExpenseRoutes.Add,
            request,
            cancellationToken
        );
        return response;
    }

    public static async Task<ExpenseDto> AddExpenseAndReturnDto(
        this HttpClient client,
        CreateExpenseRequest request,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage response = await client.AddExpense(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            string errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException(
                $"Expense creation failed with status {response.StatusCode}. Content: {errorContent}"
            );
        }

        ApiSuccessResponse<ExpenseDto> apiResponse = await response.GetApiResponse<ExpenseDto>();
        apiResponse.Payload.ShouldNotBeNull();
        return apiResponse.Payload;
    }
}
