using Mapster;
using SpendTrackApi.Controllers.Expense;

namespace SpendTrackApi.Mapping.Expense;

internal sealed class  ExpenseMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Models.Expense, ExpenseMapping>().TwoWays();
        config.NewConfig<ExpenseRequest, Models.Expense>().TwoWays();
        config.NewConfig<Models.Expense, ExpenseResponse>()
            .Map(dest => dest.CategoryName, src => src.Category!.Name);
    }

}
