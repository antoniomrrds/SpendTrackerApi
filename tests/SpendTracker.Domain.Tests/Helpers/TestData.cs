namespace SpendTracker.Domain.Tests.Helpers;

public static class TestData
{
    public static IEnumerable<object[]> InvalidNames =>
        [
            [""],
            [null!],
            ["     "]
        ];
}