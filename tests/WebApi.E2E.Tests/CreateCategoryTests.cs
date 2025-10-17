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
        : base(factory)
    {
    }

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
    
    private static List<string> GetExpectedErrors() =>
    [
        ValidationMessages.RequiredField.FormatInvariant(Name),
        ValidationMessages.StringLengthRangeMessage.FormatInvariant(Name, 4, 150),
        ValidationMessages.WhiteSpaceOnly.FormatInvariant(Description)
    ];
}
