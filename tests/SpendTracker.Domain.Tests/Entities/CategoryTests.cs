using SpendTracker.Domain.Entities;
using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;
namespace SpendTracker.Domain.Tests.Entities;
public class CategoryTests
{
    private readonly Faker _faker = new();
    private const string FieldName = nameof(Category.Name);

    private static string ExpectedNameMessage =>
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
    public void ConstructorOrSetName_GivenInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        Category category = CreateValidCategory();

        // Act & Assert constructor
        DomainException ctorException = Should.Throw<DomainException>(() => _ = new Category(invalidName!));
        ctorException.Message.ShouldBe(ExpectedNameMessage);

        // Act & Assert setter
        DomainException setterException = Should.Throw<DomainException>(() => category.Name = invalidName!);
        setterException.Message.ShouldBe(ExpectedNameMessage);
    }
    
    [Fact]
    public void ConstructorAndSetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        // Arrange
        string nameWithoutSpaces = RandomValidName;
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";

        // Act
        Category categoryFromConstructor = new(nameWithSpaces);
        Category category = new(nameWithoutSpaces) { Name = nameWithSpaces };

        // Assert
        categoryFromConstructor.Name.ShouldBe(nameWithoutSpaces);
        category.Name.ShouldBe(nameWithoutSpaces);
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
        Category category = CreateValidCategory();
        DomainException setterException = Should.Throw<DomainException>(() => category.Description = expectedDescription);
        setterException.Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public void Constructor_GivenParameterless_ThenShouldCreateObjectWithDefaultValues()
    {
        //Act
        Category category = new();
        //Assert
        category.Id.ShouldBe(Guid.Empty);
        category.Description.ShouldBe(string.Empty);
        category.Name.ShouldBe(string.Empty);
    }
}