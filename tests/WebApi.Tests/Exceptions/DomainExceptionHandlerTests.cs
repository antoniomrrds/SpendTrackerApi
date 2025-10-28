using Application.Categories.Add;
using Application.Tests.Categories.Mock;
using Domain.Errors;
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
[Trait("Type", "Integration")]
public class DomainExceptionHandlerTest: IClassFixture<NoDbTestWebAppFactory>
{
    private readonly HttpClient _client;
    private static readonly CreateCategoryRequest CreateMockInstance = CategoriesMock.Create();
    public DomainExceptionHandlerTest(NoDbTestWebAppFactory factory)
    {
        factory.ConfigureTestServicesAction = services =>
        {
            ICreateCategoryUseCase? mockUseCase = Substitute.For<ICreateCategoryUseCase>();
            mockUseCase.Perform(Arg.Any<CreateCategoryCommand>())
                .Throws(new DomainException("domain_exception_occurred"));

            services.AddSingleton(mockUseCase);
        };

        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostCategory_WhenDomainExceptionThrown_ShouldReturn400WithValidationProblemDetails()
    {
        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync(CategoriesRoutes.Add, CreateMockInstance,
            cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        ValidationProblemDetails problem = await response
            .GetErrorResponse<ValidationProblemDetails>();        
        problem.Title.ShouldBe("Business rule violation");
        problem.Type.ShouldBe("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problem.Detail.ShouldBe("domain_exception_occurred");
        problem.Instance.ShouldBe("POST /api/categories");
    }
}