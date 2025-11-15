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
    private readonly ICategoryReaderRepository _categoryRepositoriesMock;
    private readonly CancellationToken _ct = CancellationToken.None;

    public GetAllCategoriesUseCaseTests()
    {
        _categoryRepositoriesMock = Substitute.For<ICategoryReaderRepository>();
        _sut = new GetAllCategoriesUseCase(_categoryRepositoriesMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnEmptyList()
    {
        //Arrange
        MakeGetAllAsyncReturns(_emptyCategory);
        //Act
        IEnumerable<CategoryDto> result = await _sut.Perform(cancellationToken: _ct);
        //Assert
        await _categoryRepositoriesMock.Received(1).GetAllAsync(_ct);
        result.ShouldBe(_emptyCategory);
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldReturnCategoryDtoList()
    {
        //Arrange
        MakeGetAllAsyncReturns(_getCategoriesDto);
        //Act
        IReadOnlyList<CategoryDto> result = await _sut.Perform(cancellationToken: _ct);
        //Assert
        await _categoryRepositoriesMock.Received(1).GetAllAsync(_ct);
        result.Select(x => x.Id).ShouldBe(_getCategoriesDto.Select(x => x.Id));
        result.ShouldBeAssignableTo<IReadOnlyList<CategoryDto>>();
    }

    private void MakeGetAllAsyncReturns(IReadOnlyList<CategoryDto> returnValue)
    {
        _categoryRepositoriesMock.GetAllAsync(_ct).Returns(returnValue);
    }
}
