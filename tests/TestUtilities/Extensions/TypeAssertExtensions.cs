using System.Reflection;

namespace TestUtilities.Extensions;

public static class TypeAssertExtensions
{
    public static void ShouldHavePrivateConstructor(this Type type)
    {
        bool hasPrivateConstructor = type
            .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
            .Any(c => c.IsPrivate);

        hasPrivateConstructor.ShouldBeTrue($"Expected {type.Name} to have at least one private constructor.");
    }

    public static void ShouldBeSealedAndPublic(this Type type)
    {
        type.IsSealed.ShouldBeTrue($"Expected {type.Name} to be sealed.");
        type.IsPublic.ShouldBeTrue($"Expected {type.Name} to be public.");
    }

}