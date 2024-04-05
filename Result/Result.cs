namespace Nett.Core;

public class Result<TData, TError>
{
    public TData? Data { get; init; }
    public TError? Error { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsFailure => IsSuccess is false;

    public static Result<TData, TError> Success(TData data) =>
        new() { Data = data, Error = default, IsSuccess = true};

    public static Result<TData, TError> Failure(TError error) =>
        new() { Error = error, IsSuccess = false};

    public TResult Match<TResult>(Func<TData, TResult> success, Func<TError, TResult> error) =>
        IsSuccess ? success(Data!) : error(Error!);

    public static implicit operator Result<TData, TError>(TData data) => 
        new() { Data = data, Error = default, IsSuccess = true};
    
    public static implicit operator Result<TData, TError>(TError error) => 
        new() { Data = default, Error = error, IsSuccess = false};

    public static implicit operator TData(Result<TData, TError> result) =>
        result.Data ?? throw new InvalidOperationException();
}
