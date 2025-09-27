using Bogus;
using Shouldly;
using SpendTracker.Domain.Entities;
using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;
using SpendTracker.Domain.Tests.Helpers;

namespace SpendTracker.Domain.Tests.Entities;

public class CategoryTests
{
    private readonly Faker _faker = new();
    private const string FieldName = nameof(Category.Name);

    private static string ExpectedMessage =>
        ValidationMessages.RequiredField.FormatInvariant(FieldName);

    private string RandomValidName => _faker.Name.FirstName();
    private string RandomValidDescription => _faker.Lorem.Sentence();

    private Category CreateValidCategory() => new(RandomValidName);


    [Fact]
    public void Constructor_GivenValidParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        string expectedName = RandomValidName;
        string expectedDescription = RandomValidDescription;
        // Act
        Category category = new(expectedName, expectedDescription);
        // Assert
        category.Id.ShouldNotBe(Guid.Empty);
        category.Name.ShouldBe(expectedName);
        category.Description.ShouldBe(expectedDescription);
    }

    [Theory]
    [MemberData(nameof(TestData.InvalidNames), MemberType = typeof(TestData))]
    public void Constructor_GivenInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        // Act 
        void CategoryThrow() => _ = new Category(invalidName!);
        // Assert
        DomainException exception = Should.Throw<DomainException>(CategoryThrow);
        exception.Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void Constructor_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        // Arrange
        string nameWithoutSpaces = RandomValidName;
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";
        // Act
        Category category = new(nameWithSpaces);
        // Assert
        category.Name.ShouldBe(nameWithoutSpaces);
    }

    [Fact]
    public void SetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        // Arrange
        string nameWithoutSpaces = RandomValidName;
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";
        // Act
        Category category = new(nameWithoutSpaces) { Name = nameWithSpaces };
        // Assert
        category.Name.ShouldBe(nameWithoutSpaces);
    }

    [Theory]
    [MemberData(nameof(TestData.InvalidNames), MemberType = typeof(TestData))]
    public void SetName_GivenInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        Category category = CreateValidCategory();
        // Act 
        Action CategoryThrow = () => category.Name = invalidName!;
        //Assert
        DomainException exception = Should.Throw<DomainException>(CategoryThrow);
        exception.Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void ConstructorOrSetDescription_GivenMoreThan200Chars_ThenShouldThrowDomainException()
    {
        // Arrange
        string expectedDescription = _faker.Lorem.Letter(201);
        string expectedMessage = ValidationMessages.MaxChars.FormatInvariant(nameof(Category.Description), 200);

        // Act & Assert constructor
        DomainException ctorException = Should.Throw<DomainException>(() => _ = new Category(RandomValidName,expectedDescription));
        ctorException.Message.ShouldBe(expectedMessage);

        // Act & Assert setter
        Category category = new(RandomValidName);
        DomainException setterException = Should.Throw<DomainException>(() => category.Description = expectedDescription);
        setterException.Message.ShouldBe(expectedMessage);
    }
    
}