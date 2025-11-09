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
    private readonly ICategoryCheckRepository _categoryCheckRepository;
    private readonly ICategoryWriterRepository _categoryWriterRepository;
    private readonly UpdateCategoryUseCase _sut;
    private readonly UpdateCategoryInput _input = UpdateCategoryFixture.UpdateInput();
    private readonly CancellationToken _ct = CancellationToken.None;

    public UpdateCategoryUseCaseTests()
    {
        _categoryCheckRepository = Substitute.For<ICategoryCheckRepository>();
        _categoryWriterRepository = Substitute.For<ICategoryWriterRepository>();
        _sut = new UpdateCategoryUseCase(_categoryWriterRepository, _categoryCheckRepository);
    }

    [Fact]
    public async Task Perform_WhenNameIsTaken_ShouldReturnFailureWithProperError()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(true, _input.Id);
        //Act
        Result<bool> result = await _sut.Perform(_input);
        //Assert
        await _categoryCheckRepository
            .Received(1)
            .HasCategoryWithNameAsync(
                name: _input.Name,
                excludeId: _input.Id,
                cancellationToken: _ct
            );

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCategoryToUpdateIsNotFound_ShouldReturnFailure()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(false, _input.Id);
        //Act
        Result<bool> result = await _sut.Perform(_input);
        //Assert
        await _categoryWriterRepository
            .Received(1)
            .UpdateAsync(
                Arg.Is<Category>(c =>
                    c.Id == _input.Id
                    && c.Name == _input.Name
                    && c.Description == _input.Description
                ),
                _ct
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
        await _categoryCheckRepository
            .DidNotReceive()
            .HasCategoryWithNameAsync(Arg.Any<string>(), cancellationToken: _ct);

        await _categoryWriterRepository.DidNotReceive().UpdateAsync(Arg.Any<Category>(), _ct);
    }

    [Fact]
    public async Task Perform_WhenCategoryIsValid_ShouldUpdate()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(false, _input.Id);
        MakeUpdateAsyncReturns(true);
        //Act
        Result<bool> result = await _sut.Perform(_input);
        //Assert
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBeTrue();
    }

    private void MakeHasCategoryWithNameAsyncReturns(bool returnValue, Guid? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            _categoryCheckRepository
                .HasCategoryWithNameAsync(
                    name: _input.Name,
                    excludeId: excludeId.Value,
                    cancellationToken: _ct
                )
                .Returns(returnValue);
        }
        else
        {
            _categoryCheckRepository
                .HasCategoryWithNameAsync(
                    name: _input.Name,
                    excludeId: Arg.Any<Guid?>(),
                    cancellationToken: _ct
                )
                .Returns(returnValue);
        }
    }

    private void MakeUpdateAsyncReturns(bool returnValue)
    {
        _categoryWriterRepository
            .UpdateAsync(
                Arg.Is<Category>(c =>
                    c.Id == _input.Id
                    && c.Name == _input.Name
                    && c.Description == _input.Description
                ),
                _ct
            )
            .Returns(returnValue);
    }
}
