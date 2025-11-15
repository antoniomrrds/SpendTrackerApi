using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.GetById;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.GetById;

[Trait("Type", "Unit")]
public class GetCategoryByIdUseCaseTests : TestCommon
{
    private readonly GetCategoryByIdUseCase _sut;
    private readonly ICategoryReaderRepository _categoryRepositoriesMock;
    private readonly Category _expectedCategory = CategoryFixture.GetCategory();
    private readonly CategoryDto _categoryDto = CategoryDtoFixture.GetCategoryDto();
    private readonly GetCategoryByIdInput _input;
    private readonly CancellationToken _ct = CancellationToken.None;

    public GetCategoryByIdUseCaseTests()
    {
        _input = new GetCategoryByIdInput(_expectedCategory.Id);
        _categoryRepositoriesMock = Substitute.For<ICategoryReaderRepository>();
        _sut = new GetCategoryByIdUseCase(_categoryRepositoriesMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailure()
    {
        //Arrange
        MakeGetByIdAsyncReturns(null);
        //Act
        Result<CategoryDto?> result = await _sut.Perform(_input, cancellationToken: _ct);
        //Assert
        await _categoryRepositoriesMock.Received(1).GetByIdAsync(_expectedCategory.Id, _ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_expectedCategory.Id.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldReturnSuccessAndCategory()
    {
        //Arrange
        MakeGetByIdAsyncReturns(_categoryDto);
        //Act
        Result<CategoryDto?> result = await _sut.Perform(_input, cancellationToken: _ct);
        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_categoryDto);
    }

    private void MakeGetByIdAsyncReturns(CategoryDto? returnValue)
    {
        _categoryRepositoriesMock.GetByIdAsync(_expectedCategory.Id, _ct).Returns(returnValue);
    }
}
