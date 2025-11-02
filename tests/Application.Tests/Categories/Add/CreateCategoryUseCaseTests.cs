using Application.Abstractions.Data;
using Application.Categories.Add;
using Application.Categories.Common;
using Domain.Categories;
using Domain.Errors;
using NSubstitute;
using SharedKernel;

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
    private readonly CreateCategoryCommand _command;

    private CreateCategoryCommand GenerateCommand() => new(_name, _description);

    public CreateCategoryUseCaseTests()
    {
        _name = _faker.Commerce.Categories(1)[0];
        _description = _faker.Lorem.Sentence();
        _command = GenerateCommand();
        _cancellationToken = CancellationToken.None;
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.HasCategoryWithNameAsync(_command.Name).Returns(false);

        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CreateCategoryUseCase(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnSuccessResult()
    {
        Result<CategoryDto> result = await _sut.Perform(_command);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_command.Name, _cancellationToken);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(_command.Name);
        result.Value.Description.ShouldBe(_command.Description);
    }

    [Fact]
    public async Task Perform_WhenCategoryAlreadyExists_ShouldReturnFailureWithProperError()
    {
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(_command.Name, _cancellationToken)
            .Returns(Task.FromResult(true));

        Result<CategoryDto> result = await _sut.Perform(_command);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_command.Name, _cancellationToken);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NameAlreadyExists);
    }

    [Fact]
    public async Task Perform_WhenCommandIsValid_ShouldAddTrimmedCategoryToRepository()
    {
        string name = $"  {_name}  ";
        CreateCategoryCommand command = new(name, _description);
        Result<CategoryDto> result = await _sut.Perform(command);
        string expectedName = command.Name.Trim();
        result.IsSuccess.ShouldBeTrue();

        await _categoryRepositoryMock
            .Received(1)
            .AddAsync(
                Arg.Is<Category>(category =>
                    category.Name == expectedName
                    && category.Description == command.Description
                    && category.Id != Guid.Empty
                ),
                _cancellationToken
            );
    }

    [Fact]
    public async Task Perform_WhenCategoryIsCreatedSuccessfully_ShouldCommitIUnitOfWork()
    {
        Result<CategoryDto> result = await _sut.Perform(_command);

        await _categoryRepositoryMock
            .Received(1)
            .HasCategoryWithNameAsync(_command.Name, _cancellationToken);
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWorkMock.Received(1).CommitAsync();
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public async Task Perform_WhenNameIsInvalid_ShouldThrowExceptionAndNotCallRepository(
        string invalidName
    )
    {
        CreateCategoryCommand commandWithInvalidName = new(invalidName, _description);

        await Should.ThrowAsync<DomainException>(() => _sut.Perform(commandWithInvalidName));

        await _categoryRepositoryMock
            .DidNotReceive()
            .HasCategoryWithNameAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .DidNotReceive()
            .AddAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock.DidNotReceive().CommitAsync();
    }
}
