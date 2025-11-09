using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Type", "E2E")]
public class GetByIdCategoryTests(SqliteTestWebAppFactory factory)
    : BaseIntegrationTest<SqliteTestWebAppFactory>(factory)
{
    private CategoryDto? _sut;
    private readonly CancellationToken _ct = CancellationToken.None;
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryFixture.ValidRequest();

    [Fact]
    public async Task GetCategoryById_WithCorrectId_ShouldReturnCorrectData()
    {
        //Arrange
        CategoryDto createdCategory = await HttpClient.AddCategoryAndReturnDto(
            CreateMockInstance,
            _ct
        );
        //Act
        _sut = await HttpClient.GetCategoryByIdAndReturnDto(createdCategory.Id, _ct);
        //Assert
        _sut.Id.ShouldBe(createdCategory.Id);
        _sut.Name.ShouldBe(createdCategory.Name);
        _sut.Description.ShouldBe(createdCategory.Description);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidGuidValues), MemberType = typeof(InvalidInputData))]
    public async Task GetCategoryById_WithIncorrectId_ShouldReturnBadRequest(string invalidGuid)
    {
        Uri requestUri = new($"{CategoriesRoutes.BasePath}/{invalidGuid}", UriKind.Relative);
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri, _ct);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Status.ShouldBe((int)HttpStatusCode.BadRequest);
        problemDetails.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problemDetails.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        Guid expectedGuid = Faker.Random.Guid();
        //Act
        HttpResponseMessage response = await HttpClient.GetCategoryById(expectedGuid, _ct);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
