namespace Nett.Core.Result;

public class Result<TValue, TError>
{
    public TValue? Value { get; protected init; }
    public TError? Error { get; protected init; }

    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; protected init; }
    [MemberNotNullWhen(false, nameof(Value))]
    public bool IsFailure => !IsSuccess;

    public static Result<TValue, TError> Success(TValue data) =>
        new() { Value = data, Error = default, IsSuccess = true };

    public static Result<TValue, TError> Failure(TError error) =>
        new() { Error = error, IsSuccess = false };

    public static implicit operator Result<TValue, TError>(TValue data) =>
        Success(data);

    public static implicit operator Result<TValue, TError>(TError error) =>
        Failure(error);

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> error) =>
        IsSuccess ? success(Value!) : error(Error!);
}

public class Result<T> : Result<T, Error>
{
    public static implicit operator Result<T>(T value) =>
        new() { Value = value, IsSuccess = true };

    public static implicit operator Result<T>(Error error) =>
        new() { Error = error, IsSuccess = false };
}

