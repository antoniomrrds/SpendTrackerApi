using NSubstitute;
using SharedKernel;
using SharedKernel.Abstractions.Data;
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
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseUseCaseTests()
    {
        _categoryCheckRepo = Substitute.For<ICategoryCheckRepository>();
        _expenseWriterRepo = Substitute.For<IExpenseWriterRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _sut = new CreateExpenseUseCase(_categoryCheckRepo, _expenseWriterRepo, _unitOfWork);
    }

    [Fact]
    public async Task Perform_WhenCategoryDoesNotExist_ShouldReturnFailureWithProperError()
    {
        //Arrange
        MakeCategoryExistsAsyncReturns(false);
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _categoryCheckRepo.Received(1).CategoryExistsAsync(_input.IdCategory, Ct);
        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldBe(CategoryErrors.NotFound(_input.IdCategory.ToString()));
    }

    [Fact]
    public async Task Perform_WhenCategoryExists_ShouldAddExpense()
    {
        //Arrange
        MakeCategoryExistsAsyncReturns();
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _expenseWriterRepo.Received(1).AddAsync(expense: AnyParameterForMock<Expense>(), Ct);
        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task Perform_WhenAddExpense_ShouldCommitIUnitOfWork()
    {
        //Arrange
        MakeCategoryExistsAsyncReturns();
        //Act
        Result<bool> result = await _sut.Perform(_input, Ct);
        //Assert
        await _expenseWriterRepo.Received(1).AddAsync(expense: AnyParameterForMock<Expense>(), Ct);
        result.IsSuccess.ShouldBeTrue();
        await _unitOfWork.Received(1).CommitAsync(cancellationToken: Ct);
    }

    private void MakeCategoryExistsAsyncReturns(bool returnValue = true)
    {
        _categoryCheckRepo.CategoryExistsAsync(_input.IdCategory, Ct).Returns(returnValue);
    }
}
