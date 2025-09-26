using Bogus;
using Shouldly;
using SpendTracker.Domain.Entities;
using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;

namespace SpendTracker.Domain.Tests.Entities;

public class CategoryTests
{
 
    private readonly Faker _faker = new();
    [Fact]
    public void Constructor_GivenWithValidName_ThenShouldSetNamePropertyCorrectly()
    {
        //Arrange
        string nameExpected = _faker.Name.FirstName();
        
        //Act
        Category category = new(nameExpected);
        
        //Assert
        category.Name.ShouldBe(nameExpected);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("     ")]
    public void Constructor_GivenWithInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        //Arrange
        string expectedMessage = ValidationMessages.RequiredField.FormatInvariant(nameof(Category.Name));
        
        //Act
        DomainException exception = Should.Throw<DomainException>(() => new Category(invalidName!));
        
        //Assert
        exception.Message.ShouldBe(expectedMessage); 
    }
}
