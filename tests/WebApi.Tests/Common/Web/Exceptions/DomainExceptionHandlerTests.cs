using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Exceptions;
using WebApi.Domain.Errors;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories;
using WebApi.Tests.Features.Categories.Add;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;

namespace WebApi.Tests.Common.Web.Exceptions;

public class DomainExceptionHandlerTests : IClassFixture<NoDbTestWebAppFactory>
{
    private readonly HttpClient _client;
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();

    public DomainExceptionHandlerTests(NoDbTestWebAppFactory factory)
    {
        factory.ConfigureTestServicesAction = services =>
        {
            ICreateCategoryUseCase? mockUseCase = Substitute.For<ICreateCategoryUseCase>();
            mockUseCase
                .Perform(Arg.Any<CreateCategoryInput>())
                .Throws(new DomainException("domain_exception_occurred"));

            services.AddSingleton(mockUseCase);
        };

        _client = factory.CreateClient();
    }

    [Fact]
    [Trait("Type", "Unit")]
    public async Task TryHandleAsync_ShouldReturnFalse_WhenNotDomainException()
    {
        IProblemDetailsService? problemDetailsService = Substitute.For<IProblemDetailsService>();
        ILogger<DomainExceptionHandler>? logger = Substitute.For<ILogger<DomainExceptionHandler>>();
        DefaultHttpContext httpContext = new();
        InvalidOperationException exception = new("generic_error");

        DomainExceptionHandler handler = new(problemDetailsService, logger);

        bool result = await handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.ShouldBeFalse();
        await problemDetailsService.DidNotReceive().TryWriteAsync(Arg.Any<ProblemDetailsContext>());
    }

    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenDomainExceptionThrown_ShouldReturn400WithValidationProblemDetails()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            CategoriesRoutes.Add,
            CreateMockInstance,
            cancellationToken: TestContext.Current.CancellationToken
        );

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails problem =
            await response.GetErrorResponse<ValidationProblemDetails>();
        problem.Title.ShouldBe("Business rule violation");
        problem.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problem.Detail.ShouldBe("domain_exception_occurred");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}
