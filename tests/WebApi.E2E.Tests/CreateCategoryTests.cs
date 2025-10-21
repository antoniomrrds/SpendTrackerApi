using Application.Categories.Add;
using Application.Tests.Categories.Mock;
using Domain.Categories;
using Domain.Resources;
using SharedKernel.Extensions;
using System.Net;
using System.Net.Http.Json;
using WebApi.Controllers.Categories.Add;
using WebApi.E2E.Tests.Extensions;
using WebApi.E2E.Tests.Factories;
using WebApi.Responses.Errors;
using WebApi.Responses.Success;

namespace WebApi.E2E.Tests;

[Trait("Type", "E2E")]
public class CreateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private const string Name = "Name";
    private const string Description = "Description";

    private HttpResponseMessage? _sut;
    private static readonly CreateCategoryRequest CreateMockInstance = CategoriesMock.Create();

    private async Task<HttpResponseMessage> AddCategory(CreateCategoryRequest request)
    {
        return await HttpClient
            .PostAsJsonAsync(CategoriesRoutes.Add, request, CancellationToken);
    }

    public CreateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory)
    { }

    [Fact]
    public async Task PostCategory_WithInvalidData_ShouldReturnBadRequest()
    {
        //Arrange
        CreateCategoryRequest invalidRequest = new(
            Name: string.Empty,
            Description: "             "
        );

        //Act
        _sut = await AddCategory(invalidRequest);
        //Assert
        _sut.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ApiValidationErrorsResponse problemDetails = await _sut.GetErrorResponse<ApiValidationErrorsResponse>();

        problemDetails.ShouldSatisfyAllConditions(
            () => problemDetails.Errors.ShouldNotBeNull(),
            () => problemDetails.Errors.Keys.ShouldBe([Name, Description])
        );

        List<string> expectedErrors = GetExpectedErrors();

        List<string> actualErrors =
        [
            ..from errorGroup in problemDetails.Errors.Values
            from error in errorGroup
            select error
        ];

        actualErrors.ShouldSatisfyAllConditions(
            () => actualErrors.Count.ShouldBe(expectedErrors.Count),
            () => actualErrors.ShouldBe(expectedErrors)
        );
    }

    [Fact]
    public async Task PostCategory_WithExistingName_ShouldReturnConflict()
    {

        //Add a category to the database
        HttpResponseMessage addCategory = await AddCategory(CreateMockInstance);
        addCategory.StatusCode.ShouldBe(HttpStatusCode.OK);

        //Act
        _sut = await AddCategory(CreateMockInstance);
        //Assert
        _sut.StatusCode.ShouldBe(HttpStatusCode.Conflict);
        ApiErrorResponse errorResponse = await _sut.GetErrorResponse<ApiErrorResponse>();
        errorResponse.ShouldSatisfyAllConditions(
            () => errorResponse.IsSuccess.ShouldBeFalse(),
            () => errorResponse.Error.ShouldBe(CategoryErrors.CategoryNameAlreadyExists.Description)
        );
    }

    [Fact]
    public async Task PostCategory_WithValidData_ShouldReturnCorrectData()
    {
        await ResetDatabaseAsync();

        //Act
        _sut = await AddCategory(CreateMockInstance);
        //Assert
        _sut.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<CreateCategoryResult> result = await _sut.GetApiResponse<CreateCategoryResult>();
        result.IsSuccess.ShouldBeTrue();
        result.Payload?.Id.ShouldNotBe(Guid.Empty);
    }

    private static List<string> GetExpectedErrors() =>
    [
        ValidationMessages.RequiredField.FormatInvariant(Name),
        ValidationMessages.StringLengthRangeMessage.FormatInvariant(Name, 4, 150),
        ValidationMessages.WhiteSpaceOnly.FormatInvariant(Description)
    ];
}