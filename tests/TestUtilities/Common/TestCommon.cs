using NSubstitute;

namespace TestUtilities.Common;

public abstract class TestCommon
{
    protected static CancellationToken AnyCancellationToken => Arg.Any<CancellationToken>();

    protected static CancellationToken CancellationToken => CancellationToken.None;
    protected Faker Faker { get; } = FakerHelper.Faker;
}
