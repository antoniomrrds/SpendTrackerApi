using NSubstitute;
using NSubstitute.ReturnsExtensions;
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

    public GetCategoryByIdUseCaseTests()
    {
        _input = new GetCategoryByIdInput(_expectedCategory.Id);
        _categoryRepositoriesMock = Substitute.For<ICategoryReaderRepository>();
        _categoryRepositoriesMock
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken)
            .Returns(_categoryDto);
        _sut = new GetCategoryByIdUseCase(_categoryRepositoriesMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailure()
    {
        _categoryRepositoriesMock
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken)
            .ReturnsNull();
        Result<CategoryDto?> result = await _sut.Perform(_input);
        await _categoryRepositoriesMock
            .Received(1)
            .GetByIdAsync(_expectedCategory.Id, AnyCancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_expectedCategory.Id.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldReturnSuccessAndCategory()
    {
        Result<CategoryDto?> result = await _sut.Perform(_input);
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe(_categoryDto);
    }
}
