using Application.Categories.Common;
using Application.Categories.GetById;
using Application.Tests.Categories.Mock;
using Domain.Categories;
using Domain.Errors;
using Domain.Tests.Categories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SharedKernel;
using TestUtilities.Common;

namespace Application.Tests.Categories.GetById;

[Trait("Type", "Unit")]
public class GetByIdUseCaseTests : TestCommon
{
    private readonly GetByIdUseCase _sut;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly Category _expectedCategory = MockCategory.Valid();
    private readonly CategoryDto _categoryDto = MockCategoryDto.Valid();

    public GetByIdUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken)
            .Returns(_categoryDto);
        _sut = new GetByIdUseCase(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Perform_WhenGuidIsEmpty_ShouldReturnFailure()
    {
        //Arrange
        Guid invalidGuid = Guid.Empty;
        //Act
        Result<CategoryDto?> result = await _sut.Perform(invalidGuid);
        //Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CommonErrors.GuidInvalid);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailure()
    {
        //Arrange
        _categoryRepositoryMock
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken)
            .ReturnsNull();
        //Act
        Result<CategoryDto?> result = await _sut.Perform(_expectedCategory.Id);
        //Assert
        await _categoryRepositoryMock
            .Received(1)
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_expectedCategory.Id.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldReturnSuccessAndCategory()
    {
        //Act
        Result<CategoryDto?> result = await _sut.Perform(_expectedCategory.Id);
        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_categoryDto);
    }
}
