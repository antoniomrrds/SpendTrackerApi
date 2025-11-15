using NSubstitute;
using SharedKernel;
using SharedKernel.Abstractions.Data;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Domain.Errors;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;

namespace WebApi.Tests.Features.Categories.Create;

[Trait("Type", "Unit")]
public class CreateCategoryUseCaseTests : TestCommon
{
    private readonly ICategoryWriterRepository _categoryWriterRepository;
    private readonly ICategoryCheckRepository _categoryCheckRepository;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateCategoryUseCase _sut;
    private readonly CreateCategoryInput _input = CreateCategoryFixture.CategoryInput();
    private readonly CancellationToken _ct = CancellationToken.None;

    public CreateCategoryUseCaseTests()
    {
        _categoryWriterRepository = Substitute.For<ICategoryWriterRepository>();
        _categoryCheckRepository = Substitute.For<ICategoryCheckRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CreateCategoryUseCase(
            _categoryWriterRepository,
            _categoryCheckRepository,
            _unitOfWorkMock
        );
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnSuccessResult()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(false);
        //Act
        Result<CategoryDto> result = await _sut.Perform(_input, cancellationToken: _ct);
        //Assert
        await _categoryCheckRepository
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, Arg.Any<Guid?>(), _ct);
        await _categoryWriterRepository.Received(1).AddAsync(Arg.Any<Category>(), _ct);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(_input.Name);
        result.Value.Description.ShouldBe(_input.Description);
    }

    [Fact]
    public async Task Perform_WhenCategoryAlreadyExists_ShouldReturnFailureWithProperError()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(true);
        //Act
        Result<CategoryDto> result = await _sut.Perform(_input, cancellationToken: _ct);
        //Assert
        await _categoryCheckRepository
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, Arg.Any<Guid?>(), _ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCommandIsValid_ShouldAddTrimmedCategoryToRepository()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(false);
        string name = $"  {_input.Name}  ";
        CreateCategoryInput input = new() { Name = name, Description = _input.Description };
        //Act
        Result<CategoryDto> result = await _sut.Perform(input, cancellationToken: _ct);
        string expectedName = input.Name.Trim();
        //Assert
        result.IsSuccess.ShouldBeTrue();
        await _categoryWriterRepository
            .Received(1)
            .AddAsync(
                Arg.Is<Category>(category =>
                    category.Name == expectedName
                    && category.Description == input.Description
                    && category.Id != Guid.Empty
                ),
                _ct
            );
    }

    [Fact]
    public async Task Perform_WhenCategoryIsCreatedSuccessfully_ShouldCommitIUnitOfWork()
    {
        //Arrange
        MakeHasCategoryWithNameAsyncReturns(false);
        //Act
        Result<CategoryDto> result = await _sut.Perform(_input, cancellationToken: _ct);
        //Assert
        await _categoryCheckRepository
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, Arg.Any<Guid?>(), _ct);
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWorkMock.Received(1).CommitAsync(cancellationToken: _ct);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public async Task Perform_WhenNameIsInvalid_ShouldThrowExceptionAndNotCallRepository(
        string invalidName
    )
    {
        //Arrange
        CreateCategoryInput inputWithInvalidName = new()
        {
            Name = invalidName,
            Description = _input.Description,
        };
        //Act
        await Should.ThrowAsync<DomainException>(() =>
            _sut.Perform(inputWithInvalidName, cancellationToken: _ct)
        );
        //Assert
        await _categoryCheckRepository
            .DidNotReceive()
            .HasCategoryWithNameAsync(_input.Name, Arg.Any<Guid?>(), _ct);

        await _categoryWriterRepository.DidNotReceive().AddAsync(Arg.Any<Category>(), _ct);

        await _unitOfWorkMock.DidNotReceive().CommitAsync(cancellationToken: _ct);
    }

    private void MakeHasCategoryWithNameAsyncReturns(bool returnValue)
    {
        _categoryCheckRepository
            .HasCategoryWithNameAsync(_input.Name, Arg.Any<Guid?>(), _ct)
            .Returns(returnValue);
    }
}
