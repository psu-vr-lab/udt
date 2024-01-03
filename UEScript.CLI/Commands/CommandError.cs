using UEScript.Utils.Results;

namespace UEScript.CLI.Commands;

public class CommandError : Error
{
    public CommandError(string message) : base(message)
    {
    }
    
    public static CommandError UProjectFileNotFound(FileInfo uprojectFile)
        => new CommandError($"Uproject file not found: {uprojectFile.FullName}");

    public static CommandError FileNotFound(FileInfo file)
        => new CommandError("File not found: " + file.FullName);
    
    public static CommandError PathIsNotDirectory(string path)
        => new CommandError($"Path is not a directory: {path}");
    
    public static CommandError DirectoryHasWrongName(string path)
        => new CommandError($"Path is not a containing or starting with UE_(5.2,5.3,...) directory: {path}");
    
    public static CommandError NoEngineAssociations()
        => new CommandError("You don't have any engines installed. Use `ueco engine add` to add one.");
}