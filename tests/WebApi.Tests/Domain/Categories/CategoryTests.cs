using WebApi.Domain.Categories;
using WebApi.Domain.Errors;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;

namespace WebApi.Tests.Domain.Categories;

[Trait("Type", "Unit")]
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
        string expectedName = RandomValidName;
        string expectedDescription = RandomValidDescription;
        Category category = new(expectedName, expectedDescription);
        category.Id.ShouldNotBe(Guid.Empty);
        category.Name.ShouldBe(expectedName);
        category.Description.ShouldBe(expectedDescription);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorOrSetName_GivenInvalidName_ThenShouldThrowDomainException(
        string? invalidName
    )
    {
        Category category = CreateValidCategory();

        Action callInvalidName = () => _ = new Category(invalidName!);
        callInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);

        Action callSetInvalidName = () => category.SetName(invalidName!);
        callSetInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);
    }

    [Fact]
    public void ConstructorAndSetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        string nameWithoutSpaces = RandomValidName;
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";

        Category category = new(nameWithoutSpaces);
        category.SetName(nameWithSpaces);
        category.Name.ShouldBe(nameWithoutSpaces);

        Category categoryFromConstructor = new(nameWithSpaces);
        categoryFromConstructor.Name.ShouldBe(nameWithoutSpaces);
    }

    [Fact]
    public void ConstructorOrSetDescription_GivenMoreThan200Chars_ThenShouldThrowDomainException()
    {
        string? expectedDescription = _faker.Lorem.Letter(201);

        string expectedMessage = ValidationMessages.MaxChars.FormatInvariant(
            nameof(Category.Description),
            200
        );

        Action callMoreThan200Chars = () => _ = new Category(RandomValidName, expectedDescription);
        callMoreThan200Chars.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Category category = CreateValidCategory();
        Action callSetMoreThan200Chars = () => category.SetDescription(expectedDescription);
        callSetMoreThan200Chars.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void CheckSealAndPublic_GivenClass_ThenShouldReturnTrue()
    {
        typeof(Category).ShouldBeSealedAndPublic();
    }

    [Fact]
    public void HasPrivateConstructor_GivenOnePrivateConstructor_ThenShouldReturnTrue()
    {
        typeof(Category).ShouldHavePrivateConstructor();
    }
}
