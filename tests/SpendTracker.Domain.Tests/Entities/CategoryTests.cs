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
        string expectedName = _faker.Name.FirstName();

        //Act
        Category category = new(expectedName);

        //Assert
        category.Name.ShouldBe(expectedName);
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

    [Fact]
    public void Constructor_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        //Arrange
        string nameWithoutSpaces  = _faker.Name.FirstName(); 
        string nameWithSpaces  = $"  {nameWithoutSpaces}  ";
        //Act
        Category category = new(nameWithSpaces);
        //Assert
        category.Name.ShouldBe(nameWithoutSpaces);
    }

    [Fact]
    public void SetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        //Arrange
        string nameWithoutSpaces = _faker.Name.FirstName();
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";
        //Act
        Category category = new(nameWithSpaces)
        {
            Name = nameWithSpaces
        };
        //Assert
        category.Name.ShouldBe(nameWithoutSpaces);
    }
}