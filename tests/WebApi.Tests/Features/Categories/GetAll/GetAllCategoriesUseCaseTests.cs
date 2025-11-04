using NSubstitute;
using TestUtilities.Common;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.GetAll;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.GetAll;

[Trait("Type", "Unit")]
public class GetAllCategoriesUseCaseTests : TestCommon
{
    private readonly IReadOnlyList<CategoryDto> _getCategoriesDto =
        CategoryDtoFixture.GetCategoriesDto();
    private readonly IReadOnlyList<CategoryDto> _emptyCategory = CategoryDtoFixture.EmptyList();
    private readonly GetAllCategoriesUseCase _sut;
    private readonly ICategoryRepository _categoryRepositoryMock;

    public GetAllCategoriesUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.GetAllAsync(AnyCancellationToken).Returns(_getCategoriesDto);
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
        await _categoryRepositoryMock.Received(1).GetAllAsync(AnyCancellationToken);
        result.ShouldBe(_emptyCategory);
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldReturnCategoryDtoList()
    {
        //Action
        IReadOnlyList<CategoryDto> result = await _sut.Perform();
        //Assert
        await _categoryRepositoryMock.Received(1).GetAllAsync(AnyCancellationToken);
        result.Select(x => x.Id).ShouldBe(_getCategoriesDto.Select(x => x.Id));
        result.ShouldBeAssignableTo<IReadOnlyList<CategoryDto>>();
    }
}
