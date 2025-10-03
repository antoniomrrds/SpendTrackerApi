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
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void RequiredAndTrim_GivenNullOrWhitespace_ThenShouldThrowDomainException(string? input)
    {
        //Arrange
        var expectedMessage = ValidationMessages.RequiredField.FormatInvariant("Field");
        //Act && Assert
        Action callRequiredAndTrim = () => DomainValidation.RequiredAndTrim(input!, "Field");
        callRequiredAndTrim.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void RequiredAndTrim_GivenValidInput_ThenShouldReturnTrimmed()
    {
        // Arrange
        var inputWithoutSpacesExpected = RandomValidName;
        var inputWithSpaces = $"  {inputWithoutSpacesExpected}  ";
        // Act
        var result = DomainValidation.RequiredAndTrim(inputWithSpaces, "Field");
        // Assert
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenValidLength_ThenShouldReturnTrimmed()
    {
        //Arrange
        string inputWithoutSpacesExpected = new('a', 198);
        var inputWithMaxCharsAndSpaces = $" {inputWithoutSpacesExpected} ";
        //Act
        var result = DomainValidation.MaxLength(inputWithMaxCharsAndSpaces, "Field", 200);
        //Assert
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenTooLongInput_ThenShouldThrow()
    {
        //Arrange
        string inputExceedingMaxLengthExpected = new('a', 201);
        var expectedMessage = ValidationMessages.MaxChars.FormatInvariant("Field", 200);

        //Act && Assert
        Action callMaxLengthWithTooLongInput = () => DomainValidation.MaxLength(inputExceedingMaxLengthExpected, "Field", 200);
        callMaxLengthWithTooLongInput.ShouldThrowWithMessage<DomainException>(expectedMessage);

    }

    [Fact]
    public void GreaterThan_GivenValueGreaterThanMin_ThenShouldReturnValue()
    {
        //Arrange
        const decimal value = 10;
        const decimal minValue = 0;
        //Act
        var result = DomainValidation.GreaterThan(value, "Field", minValue);
        //Assert
        result.ShouldBe(value);
        result.ShouldBeGreaterThan(minValue);
    }

    [Fact]
    public void GreaterThan_GivenValueLessThanOrEqualToMin_ThenShouldThrow()
    {
        //Arrange
        const decimal value = 50;
        const decimal minValue = 100;
        var expectedMessage = ValidationMessages.GreaterThan.FormatInvariant("Field", minValue);
        //Act && Assert
        Action callGreaterThan = () => DomainValidation.GreaterThan(value, "Field", minValue);
        callGreaterThan.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(-30)]
    public void DateIsFuture_GivenDateIsNowOrPast_ThenShouldReturnValue(int daysOffset)
    {
        //Arrange
        var expectedDate = DateTime.Today.AddDays(daysOffset);
        //Act
        var result = DomainValidation.DateIsFuture(expectedDate);
        //Assert
        result.ShouldBe(expectedDate);
    }

    [Fact]
    public void DateIsFuture_GivenDateIsFuture_ThenShouldThrow()
    {
        //Arrange
        var expectedDate = DateTime.Today.AddDays(1);
        var expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(expectedDate);

        //Act && Assert
        Action callDateIsFuture = () => DomainValidation.DateIsFuture(expectedDate);
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }
}