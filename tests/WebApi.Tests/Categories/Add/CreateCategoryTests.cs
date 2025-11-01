using Application.Categories.Common;
using Application.Tests.Categories.Add;
using Domain.Categories;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.Categories.Add;
using WebApi.Responses.Errors;
using WebApi.Tests.Extensions;
using WebApi.Tests.Factories;

namespace WebApi.Tests.Categories.Add;

[Trait("Type", "E2E")]
public class CreateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryRequestMock.Valid();

    public CreateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory) { }

    [Fact]
    public async Task PostCategory_WithInvalidData_ShouldReturnBadRequest()
    {
        //Arrange
        CreateCategoryRequest invalidRequest = CreateCategoryRequestMock.Invalid();

        //Act
        HttpResponseMessage response = await HttpClient.AddCategory(
            invalidRequest,
            CancellationToken
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();

        problemDetails.Errors.ShouldNotBeNull();
    }

    [Fact]
    public async Task PostCategory_WithExistingName_ShouldReturnConflict()
    {
        //Add a category to the database
        await HttpClient.AddCategoryAndReturnDto(CreateMockInstance, CancellationToken);

        //Act
        HttpResponseMessage response = await HttpClient.AddCategory(
            CreateMockInstance,
            CancellationToken
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ProblemDetails errorResponse = await response.GetErrorResponse<ProblemDetails>();
        errorResponse.Detail.ShouldBe(CategoryErrors.CategoryNameAlreadyExists.Description);
    }

    [Fact]
    public async Task PostCategory_WithValidData_ShouldReturnCorrectData()
    {
        await ResetDatabaseAsync();

        //Act
        CategoryDto createdCategory = await HttpClient.AddCategoryAndReturnDto(
            CreateMockInstance,
            CancellationToken
        );
        //Assert
        createdCategory.Id.ShouldNotBe(Guid.Empty);
        createdCategory.Name.ShouldBe(CreateMockInstance.Name);
        createdCategory.Description.ShouldBe(CreateMockInstance.Description);
    }
}
