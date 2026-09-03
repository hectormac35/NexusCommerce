namespace Orders.Application.Common.Results;

public class Result<T> : Result
{
    private Result(bool success, T? value, string? error)
        : base(success, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Ok(T value)
        => new(true, value, null);

    public new static Result<T> Fail(string error)
        => new(false, default, error);
}
