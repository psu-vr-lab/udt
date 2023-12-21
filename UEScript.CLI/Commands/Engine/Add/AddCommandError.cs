using UEScript.Utils.Results;

namespace UEScript.CLI.Commands.Engine.Add;

public class AddCommandError : Error
{
    private AddCommandError(string message, Exception? exception = null) : base(message, exception)
    {
    }
    
    public static AddCommandError PathIsNotDirectory(string path)
        => new AddCommandError($"Path is not a directory: {path}");
    
    public static AddCommandError DirectoryHasWrongName(string path)
        => new AddCommandError($"Path is not a containing or starting with UE_(5.2,5.3,...) directory: {path}");
}