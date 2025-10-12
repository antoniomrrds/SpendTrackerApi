using SharedKernel.Resources;
using Domain.Errors;
using Domain.Validation;

namespace Domain.Tests.Validation;
[Trait("Type", "Unit")]
public class DomainValidationTests
{
    private readonly Faker _faker = new();

    private string RandomValidName => _faker.Name.FirstName();

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidValues), MemberType = typeof(InvalidInputData))]
    public void RequiredAndTrim_GivenNullOrWhitespace_ThenShouldThrowDomainException(string? input)
    {
        //Arrange
        string expectedMessage = ValidationMessageProvider.Get(ValidationKeys.RequiredField, FieldNameProvider.Get("Field"));
        //Act && Assert
        Action callRequiredAndTrim = () => DomainValidation.RequiredAndTrim(input!, "Field");
        callRequiredAndTrim.ShouldThrowWithMessage<DomainException>(expectedMessage);
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
        string expectedMessage = ValidationMessageProvider.Get(ValidationKeys.MaxChars, FieldNameProvider.Get("Field"), 200);
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
        decimal result = DomainValidation.GreaterThan(value, "Field", minValue);
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
        string expectedMessage = ValidationMessageProvider.Get(ValidationKeys.GreaterThan, FieldNameProvider.Get("Field"), minValue);        //Act && Assert
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
        DateTime expectedDate = DateTime.Today.AddDays(daysOffset);
        //Act
        DateTime result = DomainValidation.DateIsFuture(expectedDate);
        //Assert
        result.ShouldBe(expectedDate);
    }

    [Fact]
    public void DateIsFuture_GivenDateIsFuture_ThenShouldThrow()
    {
        //Arrange
        DateTime expectedDate = DateTime.Today.AddDays(1);
        string expectedMessage = ValidationMessageProvider.Get(ValidationKeys.DateIsFuture, expectedDate.ToShortDateString());
        //Act && Assert
        Action callDateIsFuture = () => DomainValidation.DateIsFuture(expectedDate);
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }
}