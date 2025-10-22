using WebApi.Controllers.Categories.Add;

namespace Application.Tests.Categories.Mock;

public static class CategoriesMock
{ 
    public static CreateCategoryRequest Create()
    {
        Faker<CreateCategoryRequest> faker = new("pt_BR");
        return faker
            .CustomInstantiator(f => new CreateCategoryRequest(
                f.Commerce.Categories(1)[0],
                f.Lorem.Letter(200)
            )).Generate();
    }
}