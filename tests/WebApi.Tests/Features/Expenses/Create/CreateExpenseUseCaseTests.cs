using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Features.Categories.Common;
using WebApi.Features.Expenses.Create;

namespace WebApi.Tests.Features.Expenses.Create;

[Trait("Type", "Unit")]
public class CreateExpenseUseCaseTests : TestCommon
{
    private readonly CreateExpenseUseCase _sut;
    private readonly CreateExpenseInput _input = CreateExpenseFixture.ExpenseInput();
    private readonly ICategoryCheckRepository _categoryCheckRepo;

    public CreateExpenseUseCaseTests()
    {
        _categoryCheckRepo = Substitute.For<ICategoryCheckRepository>();
        _sut = new CreateExpenseUseCase(_categoryCheckRepo);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailureWithProperError()
    {
        //Arrange
        _categoryCheckRepo.CategoryExists(_input.IdCategory, Ct).Returns(true);
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _categoryCheckRepo.Received(1).CategoryExists(_input.IdCategory, Ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_input.IdCategory.ToString()));
    }
}
