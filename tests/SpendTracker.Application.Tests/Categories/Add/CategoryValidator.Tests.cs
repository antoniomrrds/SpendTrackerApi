using FluentValidation.TestHelper;
using SharedKernel.Resources;
using SpendTracker.Application.Categories.Add;

namespace SpendTracker.Application.Tests.Categories.Add;

public class CategoryValidator_Tests
{
    private readonly CategoryValidator _sut;
    private readonly Faker _faker = FakerHelper.Faker;

    public CategoryValidator_Tests()
    {
        _sut = new CategoryValidator();
    }

    [Fact]
    public void Validator_WhenNameIsInvalid_ShouldReturnError()
    {
        var command = new CreateCategoryCommand(
            Name: string.Empty,
            Description: _faker.Name.FirstName());
        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(
                ValidationMessageProvider.Get(ValidationKeys.RequiredField, FieldNameProvider.Get("Name")));
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnError()
    {
        var command = new CreateCategoryCommand(
            Name: "i",
            Description: _faker.Name.FirstName());
        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.StringLengthRangeMessage,
                FieldNameProvider.Get("Name"), 4, 150));
    }

    [Fact]
    public void Validator_WhenNameIsCorrect_ShouldReturnNameWithoutError()
    {
        var command = new CreateCategoryCommand(
            Name: _faker.Name.FirstName(),
            Description: _faker.Commerce.ProductName());
        var result = _sut.TestValidate(command);
        
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        var longDescription = new string('a', 201);
        var command = new CreateCategoryCommand(
            Name: _faker.Name.FirstName(),
            Description: longDescription);

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.MaxChars,
                FieldNameProvider.Get("Description"),200));
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        var command = new CreateCategoryCommand(
            Name: _faker.Name.FirstName(),
            Description: "   ");

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.WhitespaceOnly,
                FieldNameProvider.Get("Description")));
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotReturnError()
    {
        var command = new CreateCategoryCommand(
            Name: _faker.Name.FirstName(),
            Description: string.Empty);

        var result = _sut.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotReturnError()
    {
        var command = new CreateCategoryCommand(
            Name: _faker.Name.FirstName(),
            Description: "Valid description");

        var result = _sut.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}