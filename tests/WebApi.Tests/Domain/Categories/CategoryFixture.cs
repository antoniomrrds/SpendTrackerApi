using Bogus.Extensions;
using WebApi.Domain.Categories;
using WebApi.Tests.Helpers;

namespace WebApi.Tests.Domain.Categories;

internal static class CategoryFixture
{
    private static readonly Faker<Category> Faker = FakerHelper.CreateFaker<Category>();

    public static List<Category> GetCategories(int count, bool useNewSeed = false) =>
        GetCategoryFaker(useNewSeed).Generate(count);

    public static Category GetCategory(bool useNewSeed = false) => GetCategories(1, useNewSeed)[0];

    private static Faker<Category> GetCategoryFaker(bool useNewSeed)
    {
        int seed = useNewSeed ? SecureSeedGenerator.GetSecureSeed() : 0;
        return Faker
            .CustomInstantiator(f => new Category(
                f.Random.Guid(),
                f.Commerce.Department(),
                f.Lorem.Paragraph().ClampLength(max: 200)
            ))
            .UseSeed(seed);
    }
}
