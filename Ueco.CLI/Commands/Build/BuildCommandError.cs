using Ueco.Utils.Results;

namespace Ueco.Commands.Build;

public class BuildCommandError : Error
{
    private BuildCommandError(string message, Exception? exception = null) : base(message, exception)
    {
    }
    
    public static BuildCommandError UProjectFileNotFound(FileInfo uprojectFile) 
        => new BuildCommandError($"Uproject file not found: {uprojectFile.FullName}");
}