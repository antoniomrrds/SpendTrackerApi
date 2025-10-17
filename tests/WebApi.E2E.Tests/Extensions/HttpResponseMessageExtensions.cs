using System.Net.Http.Json;
using System.Runtime.InteropServices;
using WebApi.Extensions;

namespace WebApi.E2E.Tests.Extensions;

public static class  HttpResponseMessageExtensions
{
    internal static async Task<CustomProblemDetails> GetProblemDetails(this HttpResponseMessage response)
    {

        if (response.IsSuccessStatusCode)
        {
            throw new InvalidOleVariantTypeException("Successful response");
        }

        ApiResponse<CustomProblemDetails>? problemDetails = await response
            .Content
            .ReadFromJsonAsync<ApiResponse<CustomProblemDetails>>();
            
        problemDetails.ShouldNotBeNull();
        problemDetails.ErrorDetails.ShouldNotBeNull();
        return problemDetails.ErrorDetails;
    }
}