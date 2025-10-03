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
        var expectedName = RandomValidName;
        var expectedDescription = RandomValidDescription;
        // Act
        Category category = new(expectedName, expectedDescription);
        // Assert
        category.Id.ShouldNotBe(Guid.Empty);
        category.Name.ShouldBe(expectedName);
        category.Description.ShouldBe(expectedDescription);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorOrSetName_GivenInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        // Arrange
        var category = CreateValidCategory();

        // Act & Assert constructor
        Action callInvalidName = () => _ = new Category(invalidName!);
        callInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);

        // Act & Assert setter
        var callSetInvalidName = () => category.SetName(invalidName!);
        callSetInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);
    }

    [Fact]
    public void ConstructorAndSetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        // Arrange
        var nameWithoutSpaces = RandomValidName;
        var nameWithSpaces = $"  {nameWithoutSpaces}  ";

        // Act & Assert setter
        Category category = new(nameWithoutSpaces);
        category.SetName(nameWithSpaces);
        category.Name.ShouldBe(nameWithoutSpaces);

        // Act & Assert constructor
        Category categoryFromConstructor = new(nameWithSpaces);
        categoryFromConstructor.Name.ShouldBe(nameWithoutSpaces);
    }

    [Fact]
    public void ConstructorOrSetDescription_GivenMoreThan200Chars_ThenShouldThrowDomainException()
    {
        // Arrange
        var expectedDescription = _faker.Lorem.Letter(201);
        var expectedMessage = ValidationMessages.MaxChars.FormatInvariant(nameof(Category.Description), 200);

        // Act & Assert constructor
        Action callMoreThan200Chars = () => _ = new Category(RandomValidName, expectedDescription);
        callMoreThan200Chars.ShouldThrowWithMessage<DomainException>(expectedMessage);

        // Act & Assert setter
        var category = CreateValidCategory();
        var callSetMoreThan200Chars = () => category.SetDescription(expectedDescription);
        callSetMoreThan200Chars.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void CheckSealAndPublic_GivenClass_ThenShouldReturnTrue()
    {
        //Assert
        typeof(Category).ShouldBeSealedAndPublic();
    }

    [Fact]
    public void HasPrivateConstructor_GivenOnePrivateConstructor_ThenShouldReturnTrue()
    {
        //Assert
        typeof(Category).ShouldHavePrivateConstructor();
    }
}