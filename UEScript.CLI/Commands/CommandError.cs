using UEScript.Utils.Results;

namespace UEScript.CLI.Commands;

public class CommandError : Error
{
    private CommandError(string message) : base(message)
    {
    }
    
    public static CommandError UProjectFileNotFound(FileInfo uprojectFile)
        => new CommandError($"Uproject file not found: {uprojectFile.FullName}");

    public static CommandError FileNotFound(FileInfo file)
        => new CommandError("File not found: " + file.FullName);
}