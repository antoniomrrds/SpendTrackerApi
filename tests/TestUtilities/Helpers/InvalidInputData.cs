using System.Diagnostics.CodeAnalysis;

namespace TestUtilities.Helpers;

public static class InvalidInputData
{
    public static IEnumerable<object[]> InvalidValues =>
    [
        [""],
        [null!],
        ["     "]
    ];
}