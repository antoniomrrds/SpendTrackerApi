using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Categories.Common;
using WebApi.Features.Expenses.Common;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Domain.Expenses;

namespace WebApi.Tests.Features.Expenses.Create;

[Trait("Type", "Unit")]
public class CreateExpenseUseCaseTests : TestCommon
{
    private readonly CreateExpenseUseCase _sut;
    private readonly CreateExpenseInput _input = CreateExpenseFixture.ExpenseInput();
    private readonly ICategoryCheckRepository _categoryCheckRepo;
    private readonly IExpenseWriterRepository _expenseWriterRepo;

    public CreateExpenseUseCaseTests()
    {
        _categoryCheckRepo = Substitute.For<ICategoryCheckRepository>();
        _expenseWriterRepo = Substitute.For<IExpenseWriterRepository>();
        _sut = new CreateExpenseUseCase(_categoryCheckRepo, _expenseWriterRepo);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailureWithProperError()
    {
        //Arrange
        _categoryCheckRepo.CategoryExists(_input.IdCategory, Ct).Returns(false);
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _categoryCheckRepo.Received(1).CategoryExists(_input.IdCategory, Ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_input.IdCategory.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldAddExpense()
    {
        //Arrange
        _categoryCheckRepo.CategoryExists(_input.IdCategory, Ct).Returns(true);
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _expenseWriterRepo.Received(1).AddAsync(expense: AnyParameterForMock<Expense>(), Ct);
        result.IsSuccess.ShouldBeTrue();
    }
}
