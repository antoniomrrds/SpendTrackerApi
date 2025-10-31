using System.Net.Http.Json;
using System.Runtime.InteropServices;
using WebApi.Responses.Success;

namespace WebApi.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
    internal static async Task<TError> GetErrorResponse<TError>(this HttpResponseMessage response)
        where TError : class
    {
        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOleVariantTypeException("Successful response");
        }

        TError? errorResponse = await response.Content.ReadFromJsonAsync<TError>();
        errorResponse.ShouldNotBeNull();
        return errorResponse;
    }

    internal static async Task<ApiSuccessResponse<T>> GetApiResponse<T>(
        this HttpResponseMessage response
    )
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOleVariantTypeException("Failed response");
        }

        ApiSuccessResponse<T>? apiResponse = await response.Content.ReadFromJsonAsync<
            ApiSuccessResponse<T>
        >();

        apiResponse.ShouldNotBeNull();
        return apiResponse;
    }
}
