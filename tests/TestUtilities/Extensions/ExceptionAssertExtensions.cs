namespace TestUtilities.Extensions;

public static class ExceptionAssertExtensions
{
    public static void ShouldThrowWithMessage<TException>(this Action action, string expectedMessage)
        where TException : Exception
    {
        var exception = Should.Throw<TException>(action);
        exception.Message.ShouldBe(expectedMessage);
    }
    public static async Task ShouldThrowWithMessageAsync<TException>(this Func<Task> action, string expectedMessage)
        where TException : Exception
    {
        var exception = await Should.ThrowAsync<TException>(action);
        exception.Message.ShouldBe(expectedMessage);
    }
}