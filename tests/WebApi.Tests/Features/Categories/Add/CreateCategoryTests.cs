using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;

namespace WebApi.Tests.Features.Categories.Add;

[Trait("Type", "E2E")]
public class CreateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryRequestFixture.Valid();

    public CreateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task PostCategory_WithInvalidData_ShouldReturnBadRequest()
    {
        CreateCategoryRequest invalidRequest = CreateCategoryRequestFixture.Invalid();

        HttpResponseMessage response = await HttpClient.AddCategory(
            invalidRequest,
            CancellationToken
        );
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();

        problemDetails.Errors.ShouldNotBeNull();
    }

    [Fact]
    public async Task PostCategory_WithExistingName_ShouldReturnConflict()
    {
        await HttpClient.AddCategoryAndReturnDto(CreateMockInstance, CancellationToken);

        HttpResponseMessage response = await HttpClient.AddCategory(
            CreateMockInstance,
            CancellationToken
        );
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
            CancellationToken
        );
        createdCategory.Id.ShouldNotBe(Guid.Empty);
        createdCategory.Name.ShouldBe(CreateMockInstance.Name);
        createdCategory.Description.ShouldBe(CreateMockInstance.Description);
    }
}
