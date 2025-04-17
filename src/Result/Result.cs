namespace Nett.Core.Result;

public class Result<TData, TError>
{
    public TData? Data { get; private init; }
    public TError? Error { get; private init; }
    public bool IsSuccess { get; private init; }
    public bool IsFailure => IsSuccess is false;

    public static Result<TData, TError> Success(TData data) =>
        new() { Data = data, Error = default, IsSuccess = true};

    public static Result<TData, TError> Failure(TError error) =>
        new() { Error = error, IsSuccess = false};

    public static implicit operator Result<TData, TError>(TData data) => 
        Success(data);
    
    public static implicit operator Result<TData, TError>(TError error) => 
        Failure(error);

    public TResult Match<TResult>(Func<TData, TResult> success, Func<TError, TResult> error) =>
        IsSuccess ? success(Data!) : error(Error!);
}
