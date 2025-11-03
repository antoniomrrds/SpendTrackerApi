using FluentValidation.TestHelper;
using WebApi.Domain.Extensions;
using WebApi.Domain.Resources;
using WebApi.Features.Categories.Create;

namespace Application.Tests.Categories.Add;

[Trait("Type", "Unit")]
public class CreateCategoryValidatorTests
{
    private readonly CreateCategoryValidator _sut = new();
    private static readonly Faker Faker = FakerHelper.Faker;

    private readonly string _validName = Faker.Commerce.ProductName();
    private readonly string _validDescription = Faker.Lorem.Letter(200);

    private CreateCategoryInput BuildCommand(string? name = null, string? description = null) =>
        new(name ?? _validName, description ?? _validDescription);

    [Fact]
    public void Validator_WhenNameIsInvalid_ShouldReturnError()
    {
        CreateCategoryInput input = BuildCommand(name: string.Empty);

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(ValidationMessages.RequiredField.FormatInvariant("Name"));
    }

    [Fact]
    public void Validator_WhenNameIsTooShort_ShouldReturnError()
    {
        CreateCategoryInput input = BuildCommand(name: "i");

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage(
                ValidationMessages.StringLengthRangeMessage.FormatInvariant("Name", 4, 150)
            );
    }

    [Fact]
    public void Validator_WhenNameIsCorrect_ShouldReturnNameWithoutError()
    {
        CreateCategoryInput input = BuildCommand();

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Validator_WhenDescriptionIsTooLong_ShouldReturnError()
    {
        string longDescription = new('a', 201);
        CreateCategoryInput input = BuildCommand(description: longDescription);

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.MaxChars.FormatInvariant("Description", 200));
    }

    [Fact]
    public void Validator_WhenDescriptionIsOnlyWhitespace_ShouldReturnError()
    {
        CreateCategoryInput input = BuildCommand(description: "   ");

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result
            .ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.WhiteSpaceOnly.FormatInvariant("Description"));
    }

    [Fact]
    public void Validator_WhenDescriptionIsEmpty_ShouldNotReturnError()
    {
        CreateCategoryInput input = BuildCommand(description: string.Empty);

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    [Fact]
    public void Validator_WhenDescriptionIsValid_ShouldNotReturnError()
    {
        CreateCategoryInput input = BuildCommand();

        TestValidationResult<CreateCategoryInput>? result = _sut.TestValidate(input);
        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }
}
