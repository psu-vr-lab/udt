using Microsoft.Extensions.Logging;

namespace UEScript.Utils.Results;

public class Result<TValue, TError> where TError : Error
{
    private readonly TValue? _value;
    private readonly Error[] _errors;

    protected Result(TValue value)
    {
        _value = value;
        _errors = Array.Empty<Error>();
    }

    protected Result(params Error[]? errors)
    {
        _value = default;
        _errors = errors ?? Array.Empty<Error>();
    }
    
    public bool IsSuccess => !_errors.Any();
    
    public TValue? GetValue() => _value;
    public Error[] GetErrors() => _errors;
    
    public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(value);
    public static Result<TValue, TError> Error(params TError[] error) => new Result<TValue, TError>(error);
    
    public static implicit operator Result<TValue, TError>(TValue value) => Ok(value);
    public static implicit operator Result<TValue, TError>(TError[] errors) => Error(errors);
    public static implicit operator Result<TValue, TError>(TError error) => Error(error);
}