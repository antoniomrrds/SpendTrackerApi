using NSubstitute;

namespace TestUtilities.Common;

public abstract class TestCommon
{
    public static T AnyParameterForMock<T>() => Arg.Any<T>();

    protected virtual CancellationToken Ct => CancellationToken.None;

    protected static Faker Faker => FakerHelper.Faker;
}
