using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;

namespace UEScript.CLI.Commands;

public static class CommonCommandMethods
{
    public static Result<FileInfo, CommandError> GetUprojectFile(FileInfo file, ILogger logger)
    {
        var uprojectFile = file;

        if (!Path.Exists(uprojectFile.FullName))
        {
            return CommandError.FileNotFound(file);
        }
        
        if (file.Extension != ".uproject" && file.Directory is not null)
        {
            logger.LogTrace("Searching for uproject file in {0}...", file.FullName);
            uprojectFile = file.Directory.GetFiles("*.uproject", SearchOption.TopDirectoryOnly).FirstOrDefault();
        }
        
        if (uprojectFile is null || !uprojectFile.Exists)
        {
            return CommandError.UProjectFileNotFound(uprojectFile ?? file);
        }
        
        logger.LogTrace("Unreal Engine project detected: {uprojectFile}", uprojectFile);
        
        return Result<FileInfo, CommandError>.Ok(uprojectFile);
    }
}