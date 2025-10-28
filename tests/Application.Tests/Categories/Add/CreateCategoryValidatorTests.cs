using FluentValidation.TestHelper;
using Application.Categories.Add;
using Domain.Extensions;
using Domain.Resources;

namespace Application.Tests.Categories.Add;
[Trait("Type", "Unit")]
public class CreateCategoryValidatorTests
{
    private readonly CreateCategoryValidator _sut = new();
    private static readonly Faker Faker = FakerHelper.Faker;

    private readonly string _validName = Faker.Commerce.ProductName();
    private readonly string _validDescription = Faker.Lorem.Letter(200);
    
    private CreateCategoryCommand BuildCommand(string? name = null, string? description = null)
        => new(name ?? _validName, description ?? _validDescription);
    
    [Fact]
    public void Validator_WhenNameIsInvalid_ShouldReturnError()
    {
        CreateCategoryCommand command = BuildCommand(name: string.Empty);
        
        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(ValidationMessages.RequiredField.FormatInvariant("Name"));
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnError()
    {
        CreateCategoryCommand command = BuildCommand(name: "i");
        
        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Name", 4, 150));
    }

    [Fact]
    public void Validator_WhenNameIsCorrect_ShouldReturnNameWithoutError()
    {
        CreateCategoryCommand command = BuildCommand();

        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);
        
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        string longDescription = new('a', 201);
        CreateCategoryCommand command = BuildCommand(description: longDescription);
        
        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.MaxChars.FormatInvariant("Description", 200));
    }
    
    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        CreateCategoryCommand command = BuildCommand(description: "   ");

        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.WhiteSpaceOnly.FormatInvariant("Description"));
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotReturnError()
    {
        CreateCategoryCommand command = BuildCommand(description: string.Empty);

        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotReturnError()
    {
        CreateCategoryCommand command = BuildCommand();


        TestValidationResult<CreateCategoryCommand>? result = _sut.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}