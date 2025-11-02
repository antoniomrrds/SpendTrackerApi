using System.Diagnostics.CodeAnalysis;

namespace SharedKernel;

public class Result
{
    protected internal Result(bool isSuccess, FailureReason error)
    {
        if (isSuccess && error != FailureReason.None || !isSuccess && error == FailureReason.None)
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public FailureReason Error { get; }

    public static Result Success() => new(true, FailureReason.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, FailureReason.None);

    public static Result Failure(FailureReason error) => new(false, error);

    public static Result<TValue> Failure<TValue>(FailureReason error) => new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, FailureReason error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "The value of a failure result can't be accessed."
            );

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(FailureReason.NullValue);

    public static implicit operator Result<TValue>(FailureReason error) => Failure<TValue>(error);
}
