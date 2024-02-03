namespace UEScript.Utils.Results;

public class Error(string message, Exception? exception = null)
{
    public Error(Exception exception) : this(exception.Message, exception)
    {
    }
    
    public string GetMessage() => message;
    public Exception? GetException() => exception;
}