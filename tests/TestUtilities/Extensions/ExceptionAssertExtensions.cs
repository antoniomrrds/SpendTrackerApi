namespace TestUtilities.Extensions;

public static class ExceptionAssertExtensions
{
    public static void ShouldThrowWithMessage<TException>(this Action action, string expectedMessage)
        where TException : Exception
    {
        TException exception = Should.Throw<TException>(action);
        exception.Message.ShouldBe(expectedMessage);
    }
}