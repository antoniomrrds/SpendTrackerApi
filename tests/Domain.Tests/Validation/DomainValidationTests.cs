using Domain.Errors;
using Domain.Extensions;
using Domain.Resources;
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
        string expectedMessage = ValidationMessages.RequiredField.FormatInvariant("Field");
        Action callRequiredAndTrim = () => DomainValidation.RequiredAndTrim(input!, "Field");
        callRequiredAndTrim.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void RequiredAndTrim_GivenValidInput_ThenShouldReturnTrimmed()
    {
        string inputWithoutSpacesExpected = RandomValidName;
        string inputWithSpaces = $"  {inputWithoutSpacesExpected}  ";
        string result = DomainValidation.RequiredAndTrim(inputWithSpaces, "Field");
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenValidLength_ThenShouldReturnTrimmed()
    {
        string inputWithoutSpacesExpected = new('a', 198);
        string inputWithMaxCharsAndSpaces = $" {inputWithoutSpacesExpected} ";
        string result = DomainValidation.MaxLength(inputWithMaxCharsAndSpaces, "Field", 200);
        result.ShouldBe(inputWithoutSpacesExpected);
    }

    [Fact]
    public void MaxLength_GivenTooLongInput_ThenShouldThrow()
    {
        string inputExceedingMaxLengthExpected = new('a', 201);
        string expectedMessage = ValidationMessages.MaxChars.FormatInvariant("Field", 200);
        Action callMaxLengthWithTooLongInput = () =>
            DomainValidation.MaxLength(inputExceedingMaxLengthExpected, "Field", 200);
        callMaxLengthWithTooLongInput.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Fact]
    public void GreaterThan_GivenValueGreaterThanMin_ThenShouldReturnValue()
    {
        const decimal value = 10;
        const decimal minValue = 0;
        decimal result = DomainValidation.GreaterThan(value, "Field", minValue);
        result.ShouldBe(value);
        result.ShouldBeGreaterThan(minValue);
    }

    [Fact]
    public void GreaterThan_GivenValueLessThanOrEqualToMin_ThenShouldThrow()
    {
        const decimal value = 50;
        const decimal minValue = 100;
        string expectedMessage = ValidationMessages.GreaterThan.FormatInvariant("Field", minValue);
        Action callGreaterThan = () => DomainValidation.GreaterThan(value, "Field", minValue);
        callGreaterThan.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-7)]
    [InlineData(-30)]
    public void DateIsFuture_GivenDateIsNowOrPast_ThenShouldReturnValue(int daysOffset)
    {
        DateTime expectedDate = DateTime.Today.AddDays(daysOffset);
        DateTime result = DomainValidation.DateIsFuture(expectedDate);
        result.ShouldBe(expectedDate);
    }

    [Fact]
    public void DateIsFuture_GivenDateIsFuture_ThenShouldThrow()
    {
        DateTime expectedDate = DateTime.Today.AddDays(1);
        string expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(
            expectedDate.ToShortDateString()
        );
        Action callDateIsFuture = () => DomainValidation.DateIsFuture(expectedDate);
        callDateIsFuture.ShouldThrowWithMessage<DomainException>(expectedMessage);
    }
}
