using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Domain.Errors;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Update;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Type", "Unit")]
public class UpdateCategoryUseCaseTests : TestCommon
{
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly UpdateCategoryUseCase _sut;
    private readonly UpdateCategoryInput _input = UpdateCategoryFixture.UpdateInput();

    public UpdateCategoryUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.HasCategoryWithNameAsync(_input.Name).Returns(false);
        _categoryRepositoryMock
            .UpdateAsync(
                Arg.Is<Category>(c =>
                    c.Id == _input.Id
                    && c.Name == _input.Name
                    && c.Description == _input.Description
                )
            )
            .Returns(false);
        _sut = new UpdateCategoryUseCase(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Perform_WhenNameIsTaken_ShouldReturnFailureWithProperError()
    {
        //Arrange
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(
                name: _input.Name,
                excludeId: _input.Id,
                cancellationToken: CancellationToken
            )
            .Returns(true);

        //Act
        Result<bool> result = await _sut.Perform(_input);

        //Assert
        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(
                name: _input.Name,
                excludeId: _input.Id,
                cancellationToken: CancellationToken
            );

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCategoryToUpdateIsNotFound_ShouldReturnFailure()
    {
        //Arrange
        _categoryRepositoryMock.UpdateAsync(Arg.Any<Category>(), CancellationToken).Returns(true);
        //Act
        Result<bool> result = await _sut.Perform(_input);
        //Assert
        await _categoryRepositoryMock
            .Received(1)
            .UpdateAsync(
                Arg.Is<Category>(c =>
                    c.Id == _input.Id
                    && c.Name == _input.Name
                    && c.Description == _input.Description
                ),
                AnyCancellationToken
            );
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_input.Id.ToString()));
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public async Task Perform_WhenNameIsInvalid_ShouldThrowExceptionAndNotCallRepository(
        string invalidName
    )
    {
        //Arrange
        UpdateCategoryInput inputWithInvalidName = new()
        {
            Name = invalidName,
            Description = _input.Description,
        };
        //Act
        await Should.ThrowAsync<DomainException>(() => _sut.Perform(inputWithInvalidName));
        //Assert
        await _categoryRepositoryMock
            .DidNotReceive()
            .HasCategoryWithNameAsync(Arg.Any<string>(), cancellationToken: AnyCancellationToken);

        await _categoryRepositoryMock
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Category>(), AnyCancellationToken);
    }

    [Fact]
    public async Task Perform_WhenCategoryIsValid_ShouldUpdate()
    {
        //Act
        Result<bool> result = await _sut.Perform(_input);
        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeTrue();
    }
}
