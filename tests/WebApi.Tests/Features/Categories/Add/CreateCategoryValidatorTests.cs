using FluentValidation.TestHelper;
using WebApi.Features.Categories.Create;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Add;

[Trait("Type", "Unit")]
public class CreateCategoryValidatorTests
    : CategoryValidatorTestBase<CreateCategoryValidator, CreateCategoryInput>
{
    private readonly CreateCategoryInput _categoryInput = CreateCategoryFixture.CategoryInput();

    protected override CreateCategoryInput DefaultInput() =>
        new() { Name = _categoryInput.Name, Description = _categoryInput.Description };

    [Fact]
    public void Validator_WhenInputIsFullyValid_ShouldPassAllValidations()
    {
        // Arrange
        CreateCategoryInput input = DefaultInput();
        // Act
        TestValidationResult<CreateCategoryInput>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
