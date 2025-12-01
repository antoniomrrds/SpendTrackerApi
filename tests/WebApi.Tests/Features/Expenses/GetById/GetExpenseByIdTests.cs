using WebApi.Common.Web.Constants;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Expenses.Common;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Domain.Expenses;
using WebApi.Tests.Features.Expenses.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

namespace WebApi.Tests.Features.Expenses.GetById;

[Trait("Type", "Integration")]
public class GetExpenseByIdTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private ExpenseDto? _sut;
    private readonly Category _category;
    private readonly Expense _expense;

    public GetExpenseByIdTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory(true);
        _expense = ExpenseFixture.GetExpense(true);
    }

    [Fact]
    public async Task GET_ApiExpenses_GetById_WithCorrectId_ShouldReturnCorrectData()
    {
        //Arrange

        await ExpenseSeeder.AddAsync(DbContext, _expense, _category);

        CreateExpenseRequest request = new()
        {
            Description = _expense.Description,
            Amount = _expense.Amount,
            Date = _expense.Date,
            IdCategory = _category.Id,
        };

        ExpenseDto created = await HttpClient.AddExpenseAndReturnDto(request, Ct);
        //Act
        _sut = await HttpClient.GetExpenseByIdAndReturnDto(created.Id, Ct);
        //Assert
        _sut.ShouldSatisfyAllConditions(
            r => r.Id.ShouldBe(created.Id),
            r => r.Description.ShouldBe(_expense.Description),
            r => r.Amount.ShouldBe(created.Amount),
            r => r.Date.ShouldBe(created.Date),
            r => r.CategoryId.ShouldBe(created.CategoryId),
            r => r.CategoryName.ShouldBe(created.CategoryName),
            r => r.DateFormatted.ShouldBe(created.DateFormatted),
            r => r.AmountFormatted.ShouldBe(created.AmountFormatted)
        );
    }

    [Theory]
    [MemberData(nameof(InvalidInputData.InvalidGuidValues), MemberType = typeof(InvalidInputData))]
    public async Task GET_ApiExpenses_GetById_WithIncorrectId_ShouldReturn400(string invalidGuid)
    {
        Uri requestUri = new($"{ExpenseRoutes.BasePath}/{invalidGuid}", UriKind.Relative);
        HttpResponseMessage response = await HttpClient.GetAsync(requestUri, Ct);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Status.ShouldBe((int)HttpStatusCode.BadRequest);
        problemDetails.Type.ShouldBe(ProblemDetailsTypes.BadRequest);
        problemDetails.Errors.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GET_ApiExpenses_GetById_WhenExpenseDoesNotExist_ShouldReturn404()
    {
        //Arrange
        Guid expectedGuid = Guid.NewGuid();
        //Act
        HttpResponseMessage response = await HttpClient.GetExpenseById(expectedGuid, Ct);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
