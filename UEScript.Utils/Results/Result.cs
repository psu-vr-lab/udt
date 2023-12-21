namespace UEScript.Utils.Results;

public class Result<TValue, TError> where TError : Error
{
    private readonly TValue? _value;
    private readonly TError? _error;

    protected Result(TValue value)
    {
        _value = value;
        _error = default;
    }

    protected Result(TError? error)
    {
        _value = default;
        _error = error;
    }
    
    public bool IsSuccess => _error is null;
    
    public TValue? GetValue() => _value;
    public TError? GetError() => _error;
    
    public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(value);
    public static Result<TValue, TError> Error(TError error) => new Result<TValue, TError>(error);
    public static Result<TValue, TError> Error(Error error) => new Result<TValue, TError>(error as TError);
    
    public static implicit operator Result<TValue, TError>(TValue value) => Ok(value);
    public static implicit operator Result<TValue, TError>(TError error) => Error(error);
    public static implicit operator TValue (Result<TValue, TError> result) => result.GetValue()!;
    public static implicit operator TError (Result<TValue, TError> result) => result.GetError()!;
}