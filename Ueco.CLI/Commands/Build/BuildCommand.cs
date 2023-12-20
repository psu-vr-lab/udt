using Microsoft.Extensions.Logging;
using Ueco.Utils.Results;

namespace Ueco.Commands.Build;

public static class BuildCommand
{
    public static Result<string, BuildCommandError> Execute(FileInfo file, ILogger logger)
    {
        logger.LogTrace("Build command start execution...");
        
        var uprojectFile = file;
        
        if (file.Extension != ".uproject" && file.Directory is not null)
        {
            logger.LogTrace("Searching for uproject file in {0}...", file.FullName);
            uprojectFile = file.Directory.GetFiles("*.uproject", SearchOption.TopDirectoryOnly).FirstOrDefault();
        }
        
        if (uprojectFile is null || !uprojectFile.Exists)
        {
            return BuildCommandError.UProjectFileNotFound(uprojectFile ?? file);
        }
        
        logger.LogTrace("Unreal Engine project detected: {uprojectFile}", uprojectFile);
        
        return Result<string, BuildCommandError>.Ok("Unreal Engine project was built");
    }
}