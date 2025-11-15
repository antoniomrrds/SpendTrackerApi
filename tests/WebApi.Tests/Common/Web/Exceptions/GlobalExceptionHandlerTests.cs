using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories;
using WebApi.Tests.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Common.Web.Exceptions;

public class GlobalExceptionHandlerTests : BaseExceptionHandlerTest<NoDbTestWebAppFactory>
{
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();
    private readonly CancellationToken _ct = CancellationToken.None;
    private readonly ICreateCategoryUseCase _mockUseCase;

    public GlobalExceptionHandlerTests(NoDbTestWebAppFactory factory)
        : base(factory)
    {
        _mockUseCase = Substitute.For<ICreateCategoryUseCase>();
    }

    protected override void ConfigureTestServices()
    {
        ConfigureMockUseCase(
            _mockUseCase,
            _ =>
            {
                SetupUseCaseToThrow(_mockUseCase, new InvalidOperationException("generic_error"));
            }
        );
    }

    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenUnhandledExceptionThrown_ShouldReturn500WithProblemDetails()
    {
        //Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
            CategoriesRoutes.Add,
            CreateMockInstance,
            cancellationToken: _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        ProblemDetails problem = await response.GetErrorResponse<ProblemDetails>();
        problem.ShouldNotBeNull();
        problem.Title.ShouldBe("An error occurred");
        problem.Type.ShouldBe("InvalidOperationException");
        problem.Detail.ShouldBe("generic_error");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}
