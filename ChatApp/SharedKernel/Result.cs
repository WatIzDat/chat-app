namespace SharedKernel;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (!IsValidResult(isSuccess, error))
        {
            throw new ArgumentException("Invalid result.", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success()
    {
        return new(isSuccess: true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new(isSuccess: false, error);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new(value, isSuccess: true, Error.None);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new(value: default, isSuccess: false, error);
    }

    private static bool IsValidResult(bool isSuccess, Error error)
    {
        return isSuccess && error != Error.None ||
            !isSuccess && error == Error.None;
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        this.value = value;
    }

    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("The value of a failure result cannot be accessed.");
}
