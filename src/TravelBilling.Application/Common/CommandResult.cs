namespace TravelBilling.Application.Common;

public class CommandResult
{
    protected CommandResult(bool isSuccess, ApplicationError error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ApplicationError Error { get; }

    public static CommandResult Success() => new(true, ApplicationError.None);
    public static CommandResult Failure(ApplicationError error) => new(false, error);
}

public sealed class CommandResult<T> : CommandResult
{
    private CommandResult(T? value, bool isSuccess, ApplicationError error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static CommandResult<T> Success(T value) => new(value, true, ApplicationError.None);
    public static new CommandResult<T> Failure(ApplicationError error) => new(default, false, error);
}
