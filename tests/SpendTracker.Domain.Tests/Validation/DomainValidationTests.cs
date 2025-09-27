using SpendTracker.Domain.Errors;
using SpendTracker.Domain.Extensions;
using SpendTracker.Domain.Resources;
using SpendTracker.Domain.Validation;

namespace SpendTracker.Domain.Tests.Validation;

public class DomainValidationTests
{
    private readonly Faker _faker = new();

    private string RandomValidName => _faker.Name.FirstName();

    [Theory]
    [MemberData(nameof(TestData.InvalidNames), MemberType = typeof(TestData))]
    public void RequiredAndTrim_GivenNullOrWhitespace_ThenShouldThrowDomainException(string? input)
    {
        // Act
        DomainException exception = Should.Throw<DomainException>(() =>
            DomainValidation.RequiredAndTrim(input!, "Field"));
        // Assert
        exception.Message.ShouldBe(ValidationMessages.RequiredField.FormatInvariant("Field"));
    }

    [Fact]
    public void RequiredAndTrim_GivenValidInput_ThenShouldReturnTrimmed()
    {
        // Arrange
        string inputWithoutSpacesExpected = RandomValidName;
        string inputWithSpaces = $"  {inputWithoutSpacesExpected}  ";
        // Act
        string result = DomainValidation.RequiredAndTrim(inputWithSpaces, "Field");
        // Assert
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenValidLength_ThenShouldReturnTrimmed()
    {
        //Arrange
        string inputWithoutSpacesExpected = new('a', 198);
        string inputWithMaxCharsAndSpaces = $" {inputWithoutSpacesExpected} ";
        //Act
        string result = DomainValidation.MaxLength(inputWithMaxCharsAndSpaces, "Field", 200);
        //Assert
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenTooLongInput_ThenShouldThrow()
    {
        //Arrange
        string inputExceedingMaxLengthExpected = new('a', 201);
        //Act
        DomainException exception = Should.Throw<DomainException>(() =>
            DomainValidation.MaxLength(inputExceedingMaxLengthExpected, "Field", 200));
        //Assert
        exception.Message.ShouldBe(ValidationMessages.MaxChars.FormatInvariant("Field", 200));
    }
}   