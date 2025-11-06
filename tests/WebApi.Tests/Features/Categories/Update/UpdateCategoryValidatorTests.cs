using FluentValidation.TestHelper;
using WebApi.Features.Categories.Update;
using WebApi.Tests.Features.Categories.Common;

namespace WebApi.Tests.Features.Categories.Update;

[Trait("Type", "Unit")]
public class UpdateCategoryValidatorTests
    : CategoryValidatorTestBase<UpdateCategoryValidator, UpdateCategoryInput>
{
    private readonly UpdateCategoryInput _updateInput = UpdateCategoryFixture.UpdateInput();

    protected override UpdateCategoryInput DefaultInput() =>
        new() { Name = _updateInput.Name, Description = _updateInput.Description };

    [Fact]
    public void Validator_WhenInputIsFullyValid_ShouldPassAllValidations()
    {
        // Arrange
        UpdateCategoryInput input = DefaultInput();
        // Act
        TestValidationResult<UpdateCategoryInput>? result = Sut.TestValidate(input);
        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
