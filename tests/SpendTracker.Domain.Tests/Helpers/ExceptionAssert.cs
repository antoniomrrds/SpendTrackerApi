namespace SpendTracker.Domain.Tests.Helpers;

public static class ExceptionAssert
{
    public static void ShouldThrowWithMessage<TException>(Action action, string expectedMessage)
        where TException : Exception
    {
        TException exception = Should.Throw<TException>(action);
        exception.Message.ShouldBe(expectedMessage);
    }
}
