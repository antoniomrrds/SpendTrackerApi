using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Exceptions;
using WebApi.Domain.Errors;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories;
using WebApi.Tests.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Common.Web.Exceptions;

public class DomainExceptionHandlerTests : BaseExceptionHandlerTest<NoDbTestWebAppFactory>
{
    private readonly ICreateCategoryUseCase _mockUseCase;
    private readonly CancellationToken _ct = CancellationToken.None;

    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();

    public DomainExceptionHandlerTests(NoDbTestWebAppFactory factory)
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
                SetupUseCaseToThrow(_mockUseCase, new DomainException("domain_exception_occurred"));
            }
        );
    }

    [Fact]
    [Trait("Type", "Integration")]
    public async Task PostCategory_WhenDomainExceptionThrown_ShouldReturn400WithValidationProblemDetails()
    {
        //Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync(
            CategoriesRoutes.Add,
            CreateMockInstance,
            _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        ValidationProblemDetails problem =
            await response.GetErrorResponse<ValidationProblemDetails>();
        problem.Title.ShouldBe("Business rule violation");
        problem.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problem.Detail.ShouldBe("domain_exception_occurred");
        problem.Instance.ShouldBe("POST /api/categories");
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

        bool result = await handler.TryHandleAsync(httpContext, exception, _ct);

        result.ShouldBeFalse();
        await problemDetailsService
            .DidNotReceive()
            .TryWriteAsync(AnyParameterForMock<ProblemDetailsContext>());
    }
}
