namespace Ueco.Utils.Results;

public class Error
{
    private readonly string _message;
    private readonly Exception? _exception;
    
    public Error(string message, Exception? exception = null)
    {
        _message = message;
        _exception = exception;
    }
    
    public Error(Exception exception)
    {
        _message = exception.Message;
        _exception = exception;
    }
    
    public string GetMessage() => _message;
    public Exception? GetException() => _exception;
}