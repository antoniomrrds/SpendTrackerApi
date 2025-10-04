using NSubstitute;
using SpendTracker.Application.Categories.Add;
using SpendTracker.Domain.Categories;

namespace SpendTracker.Application.Tests.Categories.Add;

public class CategoryUseCaseTests
{
    private readonly Faker _faker = FakerHelper.Faker;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly CreateCategoryUseCase _sut;

    private readonly string _name;
    private readonly string _description;
    private readonly CreateCategoryCommand _command ;
    private CreateCategoryCommand GenerateCommand() => new(_name, _description);

    
    public CategoryUseCaseTests()
    { 
         _name = _faker.Commerce.Categories(1)[0]; 
        _description = _faker.Lorem.Sentence();
        _command = GenerateCommand();
        
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _categoryRepositoryMock.HasCategoryWithNameAsync(_command.Name).Returns(false);
        _sut = new CreateCategoryUseCase(_categoryRepositoryMock);
        
    }


    [Fact] public async Task  ShouldInvokeHasCategoryWithNameAsyncOnceAndReturnResult_WhenCategoryDoesNotExist()
    {
        
        //Act
        var result = await _sut.Perform(_command);

        //Assert
        await _categoryRepositoryMock.Received(1).HasCategoryWithNameAsync(_command.Name);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(_command.Name);
        result.Value.Description.ShouldBe(_command.Description);
    }

    [Fact]
    public async Task ShouldFailWhenCategoryAlreadyExists()
    {
        // Arrange
        _categoryRepositoryMock
            .HasCategoryWithNameAsync(_command.Name)
            .Returns(Task.FromResult(true));

        //Act
        var result = await _sut.Perform(_command);
        
        //Assert
        await _categoryRepositoryMock.Received(1).HasCategoryWithNameAsync(_command.Name);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.CategoryNameAlreadyExists);
    }

    [Fact]
    public async Task ShouldInstantiateCategoryAndPassItToTheRepository()
    {
        // Arrange
        var name = $"  {_name}  "; 
        var command = new CreateCategoryCommand(name, _description);
        // Act
        var result = await _sut.Perform(command);
        var expectedName = command.Name.Trim();
        // Assert
        result.IsSuccess.ShouldBeTrue();

        await _categoryRepositoryMock.Received(1).AddAsync(Arg.Is<Category>(category =>
            category.Name == expectedName &&
            category.Description == command.Description &&
            category.Id != Guid.Empty
        ));
    }
}