using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories;
using WebApi.Tests.Features.Categories.Add;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;

namespace WebApi.Tests.Common.Web.Exceptions;

public class GlobalExceptionHandlerTests : IClassFixture<NoDbTestWebAppFactory>
{
    private readonly HttpClient _client;
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();

    public GlobalExceptionHandlerTests(NoDbTestWebAppFactory factory)
    {
        factory.ConfigureTestServicesAction = services =>
        {
            ICreateCategoryUseCase? mockUseCase = Substitute.For<ICreateCategoryUseCase>();
            mockUseCase
                .Perform(Arg.Any<CreateCategoryInput>())
                .Throws(new InvalidOperationException("generic_error"));

            services.AddSingleton(mockUseCase);
        };

        _client = factory.CreateClient();
    }

    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenUnhandledExceptionThrown_ShouldReturn500WithProblemDetails()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            CategoriesRoutes.Add,
            CreateMockInstance,
            cancellationToken: TestContext.Current.CancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        ProblemDetails problem = await response.GetErrorResponse<ProblemDetails>();
        problem.ShouldNotBeNull();
        problem.Title.ShouldBe("An error occurred");
        problem.Type.ShouldBe("InvalidOperationException");
        problem.Detail.ShouldBe("generic_error");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}
