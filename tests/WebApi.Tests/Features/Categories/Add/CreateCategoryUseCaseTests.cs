using NSubstitute;
using SharedKernel;
using SharedKernel.Abstractions.Data;
using WebApi.Domain.Categories;
using WebApi.Domain.Errors;
using WebApi.Features.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Categories.Create;

namespace Application.Tests.Categories.Add;

[Trait("Type", "Unit")]
public class CreateCategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateCategoryUseCase _sut;
    private readonly CancellationToken _cancellationToken;

    private readonly string _name;
    private readonly string _description;
    private readonly CreateCategoryInput _input;

    private CreateCategoryInput GenerateCommand() => new(_name, _description);

    public CreateCategoryUseCaseTests()
    {
        _name = _faker.Commerce.Categories(1)[0];
        _description = _faker.Lorem.Sentence();
        _input = GenerateCommand();
        _cancellationToken = CancellationToken.None;
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
            .HasCategoryWithNameAsync(_input.Name, _cancellationToken);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(_input.Name);
        result.Value.Description.ShouldBe(_input.Description);
    }

    [Fact]
    public async Task Perform_WhenCategoryAlreadyExists_ShouldReturnFailureWithProperError()
    {
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(_input.Name, _cancellationToken)
            .Returns(Task.FromResult(true));

        Result<CategoryDto> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, _cancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCommandIsValid_ShouldAddTrimmedCategoryToRepository()
    {
        string name = $"  {_name}  ";
        CreateCategoryInput input = new(name, _description);
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
                _cancellationToken
            );
    }

    [Fact]
    public async Task Perform_WhenCategoryIsCreatedSuccessfully_ShouldCommitIUnitOfWork()
    {
        Result<CategoryDto> result = await _sut.Perform(_input);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_input.Name, _cancellationToken);
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWorkMock.Received(1).CommitAsync();
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public async Task Perform_WhenNameIsInvalid_ShouldThrowExceptionAndNotCallRepository(
        string invalidName
    )
    {
        CreateCategoryInput inputWithInvalidName = new(invalidName, _description);

        await Should.ThrowAsync<DomainException>(() => _sut.Perform(inputWithInvalidName));

        await _categoryRepositoryMock
            .DidNotReceive()
            .HasCategoryWithNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .DidNotReceive()
            .AddAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.DidNotReceive().CommitAsync();
    }
}
