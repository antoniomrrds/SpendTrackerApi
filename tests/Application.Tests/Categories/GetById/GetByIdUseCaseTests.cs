using Application.Categories.Common;
using Application.Categories.GetById;
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
    public GetByIdUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.GetByIdAsync(Arg.Any<Guid>(), AnyCancellationToken)
            .Returns(MockCategory.Valid());
        _sut = new GetByIdUseCase(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Perform_WhenGuidIsEmpty_ShouldReturnFailure()
    {
        //Arrange 
        Guid invalidGuid = Guid.Empty;
        //Act
        Result<Category?> result = await _sut.Perform(invalidGuid);
        //Assert
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CommonErrors.GuidInvalid);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailure()
    {
        //Arrange
        Guid validGuid = Faker.Random.Guid();
        _categoryRepositoryMock.GetByIdAsync(validGuid, AnyCancellationToken)
            .ReturnsNull();
        //Act
        Result<Category?> result = await _sut.Perform(validGuid);
        //Assert
        await _categoryRepositoryMock.Received(1).GetByIdAsync(validGuid,   
            AnyCancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(validGuid.ToString()));
    }
}