using Domain.Categories;
using Domain.Resources;
using SharedKernel.Extensions;
using System.Net;
using System.Net.Http.Json;
using WebApi.Controllers.Categories.Add;
using WebApi.E2E.Tests.Extensions;
using WebApi.E2E.Tests.Factories;
using WebApi.Extensions;

namespace WebApi.E2E.Tests;

[Trait("Type", "E2E")]
public class CreateCategoryTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private const string Name = "Name";
    private const string Description = "Description";

    public CreateCategoryTests(SqliteTestWebAppFactory factory)
        : base(factory) {}

    [Fact]
    public async Task PostCategory_WithInvalidData_ShouldReturnBadRequest()
    {
        //Arrange
        CreateCategoryRequest invalidRequest = new(
            Name: string.Empty,
            Description: "             "
        );

        //Act
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync(CategoriesRoutes.Add, invalidRequest, CancellationToken);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        CustomProblemDetails problemDetails = await response.GetProblemDetails();
        
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
        CreateCategoryRequest expectedRequest = new(
            Name: Faker.Name.FirstName(),
            Description : Faker.Lorem.Letter(200)
        );
        HttpResponseMessage response1 = await HttpClient
            .PostAsJsonAsync(CategoriesRoutes.Add, expectedRequest, CancellationToken);
        
        response1.StatusCode.ShouldBe(HttpStatusCode.OK);
        HttpResponseMessage response =  await HttpClient
            .PostAsJsonAsync(CategoriesRoutes.Add, expectedRequest, CancellationToken);  
       
       response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
       CustomProblemDetails problemDetails = await response.GetProblemDetails();
       problemDetails.ShouldSatisfyAllConditions(
           () => problemDetails.Errors.ShouldNotBeNull(),
           () => problemDetails.Errors.Count.ShouldBe(0),
           () => problemDetails.Detail.ShouldBe(CategoryErrors.CategoryNameAlreadyExists.Description)
       );
        
    }
    
    private static List<string> GetExpectedErrors() =>
    [
        ValidationMessages.RequiredField.FormatInvariant(Name),
        ValidationMessages.StringLengthRangeMessage.FormatInvariant(Name, 4, 150),
        ValidationMessages.WhiteSpaceOnly.FormatInvariant(Description)
    ];
}
