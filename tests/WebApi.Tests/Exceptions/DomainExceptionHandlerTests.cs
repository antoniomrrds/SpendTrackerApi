using System.Net;
using System.Net.Http.Json;
using Application.Categories.Add;
using Application.Tests.Categories.Mock;
using Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WebApi.Controllers.Categories.Add;
using WebApi.Exceptions;
using WebApi.Tests.Extensions;
using WebApi.Tests.Factories;

namespace WebApi.Tests.Exceptions;

public class DomainExceptionHandlerTests : IClassFixture<NoDbTestWebAppFactory>
{
    private readonly HttpClient _client;
    private static readonly CreateCategoryRequest CreateMockInstance = CategoriesMock.Create();

    public DomainExceptionHandlerTests(NoDbTestWebAppFactory factory)
    {
        factory.ConfigureTestServicesAction = services =>
        {
            ICreateCategoryUseCase? mockUseCase = Substitute.For<ICreateCategoryUseCase>();
            mockUseCase
                .Perform(Arg.Any<CreateCategoryCommand>())
                .Throws(new DomainException("domain_exception_occurred"));

            services.AddSingleton(mockUseCase);
        };

        _client = factory.CreateClient();
    }

    [Fact]
    [Trait("Type", "Unit")]
    public async Task TryHandleAsync_ShouldReturnFalse_WhenNotDomainException()
    {
        // Arrange
        IProblemDetailsService? problemDetailsService = Substitute.For<IProblemDetailsService>();
        ILogger<DomainExceptionHandler>? logger = Substitute.For<ILogger<DomainExceptionHandler>>();
        DefaultHttpContext httpContext = new();
        InvalidOperationException exception = new("generic_error");

        DomainExceptionHandler handler = new(problemDetailsService, logger);

        // Act
        bool result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
        await problemDetailsService.DidNotReceive().TryWriteAsync(Arg.Any<ProblemDetailsContext>());
    }

    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenDomainExceptionThrown_ShouldReturn400WithValidationProblemDetails()
    {
        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            CategoriesRoutes.Add,
            CreateMockInstance,
            cancellationToken: TestContext.Current.CancellationToken
        );

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails problem =
            await response.GetErrorResponse<ValidationProblemDetails>();
        problem.Title.ShouldBe("Business rule violation");
        problem.Type.ShouldBe("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problem.Detail.ShouldBe("domain_exception_occurred");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}
