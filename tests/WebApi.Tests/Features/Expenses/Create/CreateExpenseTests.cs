using Microsoft.EntityFrameworkCore;
using WebApi.Common.Web.Responses.Errors;
using WebApi.Domain.Categories;
using WebApi.Domain.Expenses;
using WebApi.Features.Categories.Create;
using WebApi.Features.Expenses.Common;
using WebApi.Features.Expenses.Create;
using WebApi.Tests.Domain.Categories;
using WebApi.Tests.Domain.Expenses;
using WebApi.Tests.Features.Categories.Create;
using WebApi.Tests.Helpers.Extensions;
using WebApi.Tests.Helpers.Factories;
using WebApi.Tests.Infrastructure.Helpers;
using WebApi.Tests.Infrastructure.Persistence.Repositories.Expenses;

namespace WebApi.Tests.Features.Expenses.Create;

[Trait("Type", "Integration")]
public class CreateExpenseTests : BaseIntegrationTest<SqliteTestWebAppFactory>
{
    private readonly Category _category;
    private readonly Expense _expense;

    private static readonly CreateExpenseRequest Request = CreateExpenseFixture.ValidRequest();

    public CreateExpenseTests(SqliteTestWebAppFactory factory)
        : base(factory)
    {
        _category = CategoryFixture.GetCategory(true);
        _expense = ExpenseFixture.GetExpense(true);
    }

    [Fact]
    public async Task POST_ApiExpenses_Create_WhenInvalidData_ShouldReturn400()
    {
        //Arrange
        CreateExpenseRequest invalidRequest = CreateExpenseFixture.InvalidRequest();
        //Act
        HttpResponseMessage response = await HttpClient.AddExpense(invalidRequest, Ct);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        ValidationCustomProblemDetails problemDetails =
            await response.GetErrorResponse<ValidationCustomProblemDetails>();
        problemDetails.Errors.ShouldNotBeNull();
    }

    [Fact]
    public async Task POST_ApiExpenses_Create_WhenCategoryIdDoesNotExist_ShouldReturn404()
    {
        //Act
        HttpResponseMessage response = await HttpClient.AddExpense(Request, Ct);
        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task POST_ApiExpenses_Create_WhenValidData_ShouldReturn200AndCorrectData()
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

        //Act
        ExpenseDto response = await HttpClient.AddExpenseAndReturnDto(request, Ct);
        //Assert
        response.ShouldNotBeNull();
        response.ShouldSatisfyAllConditions(
            r => r.Id.ShouldNotBe(Guid.Empty),
            r => r.Description.ShouldBe(_expense.Description),
            r => r.Amount.ShouldBe(_expense.Amount),
            r => r.Date.ShouldBe(_expense.Date),
            r => r.CategoryId.ShouldBe(_expense.IdCategory),
            r => r.CategoryName.ShouldBe(_category.Name),
            r => r.DateFormatted.ShouldNotBeNullOrEmpty(),
            r => r.AmountFormatted.ShouldNotBeNullOrEmpty()
        );
    }
}
