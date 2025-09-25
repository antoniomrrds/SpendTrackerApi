using Bogus;
using Shouldly;
using SpendTracker.Domain.Entities;

namespace SpendTracker.Domain.Tests.Entities;

public class CategoryTests
{

    private readonly Faker _faker = new();
    [Fact]
    public void Constructor_GivenWithValidName_ThenShouldSetNamePropertyCorrectly()
    {
        string nameExpected = _faker.Name.FirstName();
        Category category = new(nameExpected);
        category.Name.ShouldBe(nameExpected);
    }
}
