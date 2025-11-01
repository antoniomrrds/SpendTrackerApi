using Application.Categories.Common;
using Application.Tests.Categories.Add;
using WebApi.Controllers.Categories.Add;
using WebApi.Tests.Categories.Add;
using WebApi.Tests.Factories;

namespace WebApi.Tests.Categories.GetById;

[Trait("Type", "E2E")]
public class GetByIdCategoryTests(SqliteTestWebAppFactory factory)
    : BaseIntegrationTest<SqliteTestWebAppFactory>(factory)
{
    private CategoryDto? _sut;
    private static readonly CreateCategoryRequest CreateMockInstance =
        CreateCategoryRequestMock.Valid();

    [Fact]
    public async Task GetCategoryById_WithCorrectId_ShouldReturnCorrectData()
    {
        //Arrange
        CategoryDto createdCategory = await HttpClient.AddCategoryAndReturnDto(
            CreateMockInstance,
            CancellationToken
        );
        //Act
        _sut = await HttpClient.GetCategoryByIdAndReturnDto(createdCategory.Id, CancellationToken);
        //Assert
        _sut.Id.ShouldBe(createdCategory.Id);
        _sut.Name.ShouldBe(createdCategory.Name);
        _sut.Description.ShouldBe(createdCategory.Description);
    }
}
