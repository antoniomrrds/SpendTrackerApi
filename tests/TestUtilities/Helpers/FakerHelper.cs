namespace TestUtilities.Helpers;

public static class FakerHelper
{
    public static Faker Faker { get; } = new("pt_BR");
    public static Faker<T> CreateFaker<T>() where T : class => new("pt_BR");
}