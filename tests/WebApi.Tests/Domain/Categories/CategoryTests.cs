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
    private readonly Category _getCategory = CategoryFixture.GetCategory();
    private readonly Category _sut;

    public CategoryTests()
    {
        _sut = new Category(
            id: _getCategory.Id,
            name: _getCategory.Name,
            description: _getCategory.Description
        );
    }

    [Fact]
    public void Constructor_GivenValidParameters_ShouldSetPropertiesCorrectly()
    {
        _sut.Id.ShouldBe(_getCategory.Id);
        _sut.Name.ShouldBe(_getCategory.Name);
        _sut.Description.ShouldBe(_getCategory.Description);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void ConstructorOrSetName_GivenInvalidName_ThenShouldThrowDomainException(
        string? invalidName
    )
    {
        Action callInvalidName = () => _ = new Category(_getCategory.Id, invalidName!);
        callInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);

        Action callSetInvalidName = () => _sut.SetName(invalidName!);
        callSetInvalidName.ShouldThrowWithMessage<DomainException>(ExpectedNameMessage);
    }

    [Fact]
    public void ConstructorAndSetName_GivenNameWithSpaces_ThenShouldTrimSpacesFromName()
    {
        string nameWithoutSpaces = _getCategory.Name;
        string nameWithSpaces = $"  {nameWithoutSpaces}  ";

        Category category = new(_getCategory.Id, nameWithoutSpaces);
        category.SetName(nameWithSpaces);
        category.Name.ShouldBe(nameWithoutSpaces);

        Category categoryFromConstructor = new(_getCategory.Id, nameWithSpaces);
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

        Action callMoreThan200Chars = () =>
            _ = new Category(_getCategory.Id, _getCategory.Name, expectedDescription);
        callMoreThan200Chars.ShouldThrowWithMessage<DomainException>(expectedMessage);

        Action callSetMoreThan200Chars = () => _sut.SetDescription(expectedDescription);
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
