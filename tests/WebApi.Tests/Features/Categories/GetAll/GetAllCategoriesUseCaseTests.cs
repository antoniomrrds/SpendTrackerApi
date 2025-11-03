using NSubstitute;
using TestUtilities.Common;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.GetAll;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.GetAll;

[Trait("Type", "Unit")]
public class GetAllCategoriesUseCaseTests : TestCommon
{
    private readonly IEnumerable<CategoryDto> _categoryDto = CategoryDtoMock.ValidList();
    private readonly IEnumerable<CategoryDto> _emptyCategory = CategoryDtoMock.EmptyList();
    private readonly GetAllCategoriesUseCase _sut;
    private readonly ICategoryRepository _categoryRepositoryMock;

    public GetAllCategoriesUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.GetAllAsync(AnyCancellationToken).Returns(_categoryDto);
        _sut = new GetAllCategoriesUseCase(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnEmptyList()
    {
        //Arrange
        _categoryRepositoryMock.GetAllAsync(AnyCancellationToken).Returns(_emptyCategory);
        //Action
        IEnumerable<CategoryDto> result = await _sut.Perform();
        //Assert
        result.ShouldBe([]);
    }
}
