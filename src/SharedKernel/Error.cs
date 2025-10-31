namespace SharedKernel;

public record FailureReason(string Code, string Description)
{
    public static readonly FailureReason None = new(string.Empty, string.Empty);
    public static readonly FailureReason NullValue = new(
        "FailureReason.NullValue",
        "Null value was provided"
    );

    public static implicit operator Result(FailureReason error) => Result.Failure(error);

    public Result ToResult() => Result.Failure(this);
}
