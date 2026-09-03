namespace Orders.Application.Common.Results;

public class Result
{
    protected Result(bool success, string? error)
    {
        Success = success;
        Error = error;
    }

    public bool Success { get; }

    public bool Failure => !Success;

    public string? Error { get; }

    public static Result Ok()
        => new(true, null);

    public static Result Fail(string error)
        => new(false, error);
}
