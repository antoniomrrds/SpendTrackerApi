using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Domain.Categories;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

namespace WebApi.Tests.Features.Categories.Delete;

[Trait("Type", "Integration")]
public class DeleteCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private readonly Category _category;
    private readonly CancellationToken _ct = CancellationToken.None;

    public DeleteCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory(true);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidGuidValues), MemberType = typeof(InvalidInputData))]
    public async Task DELETE_ApiCategories_Delete_WhenIdIsInvalid_ShouldReturn400(
        string invalidGuid
    )
    {
        //Arrange
        Uri requestUri = new($"{CategoriesRoutes.BasePath}/{invalidGuid}", UriKind.Relative);
        //Act
        HttpResponseMessage response = await HttpClient.DeleteAsync(requestUri, _ct);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Status.ShouldBe((int)HttpStatusCode.BadRequest);
        problemDetails.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problemDetails.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public async Task DELETE_ApiCategories_Delete_WhenCategoryToDeleteIsNotFound_ShouldReturn404()
    {
        //Arrange
        Guid expectedGuid = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await HttpClient.DeleteAsync(
            CategoriesRoutes.Delete(expectedGuid),
            cancellationToken: _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DELETE_ApiCategories_Delete_WhenValidData_ShouldReturn204()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        Guid expectedGuid = _category.Id;
        //Act
        HttpResponseMessage response = await HttpClient.DeleteAsync(
            CategoriesRoutes.Delete(expectedGuid),
            cancellationToken: _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    private async Task MakeCreateCategoryAsync()
    {
        await CategorySeeder.AddAsync(DbContext, _category);
    }
}
