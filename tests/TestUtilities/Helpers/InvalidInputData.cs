namespace TestUtilities.Helpers;

public static class InvalidInputData
{
    public static IEnumerable<object[]> InvalidValues =>
        [
            [""],
            [null!],
            ["     "],
        ];

    public static IEnumerable<object[]> InvalidGuidValues =>
        [
            ["abc123"],
            ["ZZZZZZZZ-ZZZZ-ZZZZ-ZZZZ-ZZZZZZZZZZZZ"],
            ["not-a-guid"],
            ["123"],
            ["00000000-0000-0000-0000-000000000000"],
        ];
}
