using NSubstitute;
using SharedKernel;
using TestUtilities.Common;
using WebApi.Features.Expenses.Common;
using WebApi.Features.Expenses.GetById;
using WebApi.Tests.Features.Expenses.Common;

namespace WebApi.Tests.Features.Expenses.GetById;

[Trait("Type", "Unit")]
public class GetExpenseByIdUseCaseTests : TestCommon
{
    private readonly IExpenseReaderRepository _expenseReaderRepo;
    private readonly GetExpenseByIdUseCase _sut;
    private readonly ExpenseDto _getExpenseDto = ExpenseDtoFixture.GetExpenseDto();
    private readonly GetExpenseByIdInput _input;

    public GetExpenseByIdUseCaseTests()
    {
        _input = new GetExpenseByIdInput(_getExpenseDto.Id);
        _expenseReaderRepo = Substitute.For<IExpenseReaderRepository>();
        _sut = new GetExpenseByIdUseCase(_expenseReaderRepo);
    }

    [Fact]
    public async Task Perform_WhenExpenseExists_ShouldReturnDto()
    {
        //Arrange
        _expenseReaderRepo.GetByIdAsync(AnyParameterForMock<Guid>(), Ct).Returns(_getExpenseDto);
        //Act
        Result<ExpenseDto?> result = await _sut.Perform(_input, Ct);
        //Assert
        await _expenseReaderRepo.Received(1).GetByIdAsync(AnyParameterForMock<Guid>(), Ct);
        result.ShouldSatisfyAllConditions(
            r => r.Value.Id.ShouldBe(_getExpenseDto.Id),
            r => r.Value.Description.ShouldBe(_getExpenseDto.Description),
            r => r.Value.Amount.ShouldBe(_getExpenseDto.Amount),
            r => r.Value.Date.ShouldBe(_getExpenseDto.Date),
            r => r.Value.CategoryId.ShouldBe(_getExpenseDto.CategoryId),
            r => r.Value.CategoryName.ShouldBe(_getExpenseDto.CategoryName),
            r => r.Value.DateFormatted.ShouldBe(_getExpenseDto.DateFormatted),
            r => r.Value.AmountFormatted.ShouldBe(_getExpenseDto.AmountFormatted)
        );
    }
}
