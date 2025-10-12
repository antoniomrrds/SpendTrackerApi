using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using WebApi.Controllers.Categories.Add;
using WebApi.E2E.Tests.Factories;

namespace WebApi.E2E.Tests;

public static class CategoriesRoutes 
{
    private const string BaseRoute = "api/categories";
  public static string Add => $"{BaseRoute}";
}

public class CreateCategoryTests: BaseIntegrationTest<MySqlTestWebAppFactory>
{
    public CreateCategoryTests(MySqlTestWebAppFactory factory)
        : base(factory)
    {
    }
    
    [Fact]
    public async Task PostCategory_WithInvalidName_ShouldReturnBadRequest()
    {
        //Arrange
        CreateCategoryRequest invalidRequest = new (
            Name: string.Empty,     
            Description: "A test category" 
        );

        // Act
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync(CategoriesRoutes.Add, invalidRequest,CancellationToken);
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest, 
            $"A rota '{CategoriesRoutes.Add}' deve estar acessível e a validação deve retornar 400.");
        
        ValidationProblemDetails? problemDetails = await response
            .Content.ReadFromJsonAsync<ValidationProblemDetails>(CancellationToken);
        problemDetails?.Errors.ShouldContainKey("Name");
    }
}