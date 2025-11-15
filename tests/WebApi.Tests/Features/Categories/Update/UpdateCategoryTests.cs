using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Common.Web.Responses.Success;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Persistence.Repositories.categories;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Type", "Integration")]
public class UpdateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private readonly Category _category;
    private readonly CancellationToken _ct = CancellationToken.None;
    private readonly IEnumerable<Category> _categories;

    public UpdateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory(true);
        _categories = CategoryFixture.GetCategories(3, true);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidGuidValues), MemberType = typeof(InvalidInputData))]
    public async Task PUT_ApiCategories_Update_WhenIdIsInvalid_ShouldReturn400(string invalidGuid)
    {
        //Arrange
        UpdateCategoryRequest request = new()
        {
            Name = _category.Name,
            Description = _category.Description,
        };
        Uri requestUri = new($"{CategoriesRoutes.BasePath}/{invalidGuid}", UriKind.Relative);
        //Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(requestUri, request, _ct);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Status.ShouldBe((int)HttpStatusCode.BadRequest);
        problemDetails.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problemDetails.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public async Task PUT_ApiCategories_Update_WhenInvalidCategory_ShouldReturn400()
    {
        //Arrange
        Guid expectedGuid = Guid.NewGuid();
        UpdateCategoryRequest request = new() { Name = "", Description = Faker.Lorem.Letter(201) };
        //Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(
            CategoriesRoutes.Update(expectedGuid),
            request,
            _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Status.ShouldBe((int)HttpStatusCode.BadRequest);
        problemDetails.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problemDetails.Errors.Count.ShouldBe(2);
    }

    [Fact]
    public async Task PUT_ApiCategories_Update_WhenCategoryDoesNotExist_ShouldReturn404()
    {
        //Arrange
        Guid expectedGuid = Guid.NewGuid();

        UpdateCategoryRequest request = new()
        {
            Name = _category.Name,
            Description = _category.Description,
        };
        //Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(
            CategoriesRoutes.Update(expectedGuid),
            request,
            _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PUT_ApiCategories_Update_WhenNameAlreadyExists_ShouldReturn409()
    {
        //Arrange
        await MakeCreateCategoriesAsync();
        List<Category> existingCategories = [.. _categories];
        Category categoryToUpdate = existingCategories[1];
        string categoryWithExistingName = existingCategories[0].Name;
        UpdateCategoryRequest request = new()
        {
            Name = categoryWithExistingName,
            Description = _category.Description,
        };
        //Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(
            CategoriesRoutes.Update(categoryToUpdate.Id),
            request,
            _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task PUT_ApiCategories_Update_WhenValidData_ShouldReturn200AndCorrectData()
    {
        //Arrange
        await MakeCreateCategoryAsync();
        Guid expectedGuid = _category.Id;

        UpdateCategoryRequest request = new()
        {
            Name = "Novo Nome Atualizado",
            Description = "Nova Descrição",
        };
        //Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(
            CategoriesRoutes.Update(expectedGuid),
            request,
            _ct
        );
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        ApiSuccessResponse<CategoryDto> apiResponse = await response.GetApiResponse<CategoryDto>();
        apiResponse.Payload.ShouldNotBeNull();
        apiResponse.Payload.Name.ShouldBe("Novo Nome Atualizado");
        apiResponse.Payload.Description.ShouldBe("Nova Descrição");
    }

    private async Task MakeCreateCategoriesAsync()
    {
        await CategorySeeder.AddRangeAsync(DbContext, _categories);
    }

    private async Task MakeCreateCategoryAsync()
    {
        await CategorySeeder.AddAsync(DbContext, _category);
    }
}
