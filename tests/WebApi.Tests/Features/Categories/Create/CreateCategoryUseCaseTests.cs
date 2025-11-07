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
public class CreateCategoryUseCaseTests:TestCommon
{
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateCategoryUseCase _sut;
    private readonly CreateCategoryInput _input = CreateCategoryFixture.CategoryInput();
    public CreateCategoryUseCaseTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.HasCategoryWithNameAsync(_input.Name).Returns(false);

        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CreateCategoryUseCase(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnSuccessResult()
    {
        Result<CategoryDto> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, cancellationToken: CancellationToken);
        await _categoryRepositoryMock
            .Received(1)
            .AddAsync(Arg.Any<Category>(), CancellationToken);
        
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(_input.Name);
        result.Value.Description.ShouldBe(_input.Description);
    }

    [Fact]
    public async Task Perform_WhenCategoryAlreadyExists_ShouldReturnFailureWithProperError()
    {
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(_input.Name,cancellationToken: AnyCancellationToken)
            .Returns(true);

        Result<CategoryDto> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name,cancellationToken:  AnyCancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCommandIsValid_ShouldAddTrimmedCategoryToRepository()
    {
        string name = $"  {_input.Name}  ";
        CreateCategoryInput input = new() { Name = name, Description = _input.Description };

        Result<CategoryDto> result = await _sut.Perform(input);
        string expectedName = input.Name.Trim();
        result.IsSuccess.ShouldBeTrue();

        await _categoryRepositoryMock
            .Received(1)
            .AddAsync(
                Arg.Is<Category>(category =>
                    category.Name == expectedName
                    && category.Description == input.Description
                    && category.Id != Guid.Empty
                ),
                CancellationToken
            );
    }

    [Fact]
    public async Task Perform_WhenCategoryIsCreatedSuccessfully_ShouldCommitIUnitOfWork()
    {
        Result<CategoryDto> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name,cancellationToken:   CancellationToken);
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWorkMock.Received(1).CommitAsync();
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public async Task Perform_WhenNameIsInvalid_ShouldThrowExceptionAndNotCallRepository(
        string invalidName
    )
    {
        CreateCategoryInput inputWithInvalidName = new()
        {
            Name = invalidName,
            Description = _input.Description,
        };
        
        await Should.ThrowAsync<DomainException>(() => _sut.Perform(inputWithInvalidName));

        await _categoryRepositoryMock
            .DidNotReceive()
            .HasCategoryWithNameAsync(Arg.Any<string>(),cancellationToken:  AnyCancellationToken);

        await _categoryRepositoryMock
            .DidNotReceive()
            .AddAsync(Arg.Any<Category>(), AnyCancellationToken);

        await _unitOfWorkMock.DidNotReceive().CommitAsync();
    }
}
