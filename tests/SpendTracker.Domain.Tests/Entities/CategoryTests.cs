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

    private Category CreateValidCategory() => new(RandomValidName);

    private static void AssertRequiredFieldException(Action action)
    {
        DomainException exception = Should.Throw<DomainException>(action);
        exception.Message.ShouldBe(ExpectedMessage);
    }

    [Fact]
    public void Constructor_GivenValidName_ThenShouldSetNamePropertyCorrectly()
    {
        // Arrange
        string expectedName = RandomValidName;

        // Act
        Category category = new(expectedName);

        // Assert
        category.Name.ShouldBe(expectedName);
    }

    [Theory]
    [MemberData(nameof(TestData.InvalidNames), MemberType = typeof(TestData))]
    public void Constructor_GivenInvalidName_ThenShouldThrowDomainException(string? invalidName)
    {
        // Act & Assert
        AssertRequiredFieldException(() => _ = new Category(invalidName!));
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

        // Act & Assert
        AssertRequiredFieldException(() => category.Name = invalidName!);
    }
}