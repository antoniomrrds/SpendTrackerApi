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
        //Arrange
        string expectedMessage = ValidationMessages.RequiredField.FormatInvariant("Field"); 
        //Act && Assert
        void CallRequiredAndTrim() => DomainValidation.RequiredAndTrim(input!, "Field");
        ExceptionAssert.ShouldThrowWithMessage<DomainException>(CallRequiredAndTrim, expectedMessage);
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
        string expectedMessage = ValidationMessages.MaxChars.FormatInvariant("Field", 200);
        
        //Act && Assert
        void CallMaxLengthWithTooLongInput() => DomainValidation.MaxLength(inputExceedingMaxLengthExpected, "Field", 200);
        ExceptionAssert.ShouldThrowWithMessage<DomainException>(CallMaxLengthWithTooLongInput,expectedMessage);
    
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
        string expectedMessage = ValidationMessages.GreaterThan.FormatInvariant("Field", minValue);
        //Act && Assert
        static void CallGreaterThan() => DomainValidation.GreaterThan(value, "Field", minValue); 
        ExceptionAssert.ShouldThrowWithMessage<DomainException>(CallGreaterThan, expectedMessage);
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
        string expectedMessage = ValidationMessages.DateIsFuture.FormatInvariant(expectedDate);
        
       //Act && Assert
       void CallDateIsFuture() =>  DomainValidation.DateIsFuture(expectedDate); 
       ExceptionAssert.ShouldThrowWithMessage<DomainException>(CallDateIsFuture, expectedMessage);
    }
}   