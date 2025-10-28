using Application.Categories.Add;
using Application.Tests.Categories.Mock;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Net;
using System.Net.Http.Json;
using WebApi.Controllers.Categories.Add;
using WebApi.Tests.Extensions;
using WebApi.Tests.Factories;

namespace WebApi.Tests.Exceptions;

public class GlobalExceptionHandlerTests: IClassFixture<NoDbTestWebAppFactory>
{
    private readonly HttpClient _client;
    private static readonly CreateCategoryRequest CreateMockInstance = CategoriesMock.Create();
    public GlobalExceptionHandlerTests(NoDbTestWebAppFactory factory)
    {
        factory.ConfigureTestServicesAction = services =>
        {
            ICreateCategoryUseCase? mockUseCase = Substitute.For<ICreateCategoryUseCase>();
            mockUseCase.Perform(Arg.Any<CreateCategoryCommand>())
                .Throws(new InvalidOperationException("generic_error"));

            services.AddSingleton(mockUseCase);
        };

        _client = factory.CreateClient();
    }
        
    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenUnhandledExceptionThrown_ShouldReturn500WithProblemDetails()  
    {
        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(CategoriesRoutes.Add, CreateMockInstance,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.InternalServerError);

        ProblemDetails problem = await response.GetErrorResponse<ProblemDetails>();
        problem.ShouldNotBeNull();
        problem.Title.ShouldBe("An error occurred");
        problem.Type.ShouldBe("InvalidOperationException");
        problem.Detail.ShouldBe("generic_error");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}