using Bogus.Extensions;
using Domain.Categories;

namespace Domain.Tests.Categories;

public static class MockCategory
{
    private static readonly Faker<Category> Faker = FakerHelper.CreateFaker<Category>();

    public static Category Valid()
    {
        return Faker
            .CustomInstantiator(f => new Category(
                f.Commerce.Department(),
                f.Lorem.Paragraph().ClampLength(max: 200)
            ))
            .Generate();
    }
}
