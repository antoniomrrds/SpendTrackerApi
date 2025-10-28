namespace TestUtilities.Common;

public abstract class TestCommon
{
    protected static CancellationToken CancellationToken => TestContext.Current.CancellationToken;
    protected Faker Faker { get; } = FakerHelper.Faker;
}