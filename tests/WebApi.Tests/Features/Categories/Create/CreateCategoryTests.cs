using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Type", "Integration")]
public class CreateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();
    private readonly CancellationToken _ct = CancellationToken.None;

    public CreateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task PostCategory_WithInvalidData_ShouldReturnBadRequest()
    {
        CreateCategoryRequest invalidRequest = CreateCategoryFixture.Invalid();

        HttpResponseMessage response = await HttpClient.AddCategory(invalidRequest, _ct);
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();

        problemDetails.Errors.ShouldNotBeNull();
    }

    [Fact]
    public async Task PostCategory_WithExistingName_ShouldReturnConflict()
    {
        await ResetDatabaseAsync();
        await HttpClient.AddCategoryAndReturnDto(CreateMockInstance, _ct);

        HttpResponseMessage response = await HttpClient.AddCategory(CreateMockInstance, _ct);
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ProblemDetails errorResponse = await response.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.NameAlreadyExists.Description);
    }

    [Fact]
    public async Task PostCategory_WithValidData_ShouldReturnCorrectData()
    {
        await ResetDatabaseAsync();

        CategoryDto createdCategory = await HttpClient.AddCategoryAndReturnDto(
            CreateMockInstance,
            _ct
        );
        createdCategory.Id.ShouldNotBe(Guid.Empty);
        createdCategory.Name.ShouldBe(CreateMockInstance.Name);
        createdCategory.Description.ShouldBe(CreateMockInstance.Description);
    }
}
