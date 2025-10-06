using FluentValidation.TestHelper;
using SharedKernel.Resources;
using Application.Categories.Add;

namespace Application.Tests.Categories.Add;
[Trait("Type", "Unit")]
public class CategoryValidatorTests
{
    private readonly CategoryValidator _sut = new();
    private static readonly Faker Faker = FakerHelper.Faker;

    private readonly string _validName = Faker.Commerce.ProductName();
    private readonly string _validDescription = Faker.Lorem.Letter(200);
    
    private CreateCategoryCommand BuildCommand(string? name = null, string? description = null)
        => new(name ?? _validName, description ?? _validDescription);
    
    [Fact]
    public void Validator_WhenNameIsInvalid_ShouldReturnError()
    {
        var command = BuildCommand(name: string.Empty);
        
        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(
                ValidationMessageProvider.Get(ValidationKeys.RequiredField, FieldNameProvider.Get("Name")));
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnError()
    {
        var command = BuildCommand(name: "i");
        
        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.StringLengthRangeMessage,
                FieldNameProvider.Get("Name"), 4, 150));
    }

    [Fact]
    public void Validator_WhenNameIsCorrect_ShouldReturnNameWithoutError()
    {
        var command = BuildCommand();

        var result = _sut.TestValidate(command);
        
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        var longDescription = new string('a', 201);
        var command = BuildCommand(description: longDescription);
        
        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.MaxChars,
                FieldNameProvider.Get("Description"),200));
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        var command = BuildCommand(description: "   ");

        var result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessageProvider.Get(ValidationKeys.WhitespaceOnly,
                FieldNameProvider.Get("Description")));
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotReturnError()
    {
        var command = BuildCommand(description: string.Empty);

        var result = _sut.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotReturnError()
    {
        var command = BuildCommand();


        var result = _sut.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}