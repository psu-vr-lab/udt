using Microsoft.Extensions.Logging;

namespace Ueco.Commands.Build;

public static class BuildCommand
{
    public static void Execute(FileInfo file, ILogger logger)
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
            logger.LogError("Uproject file not found: {0}", uprojectFile is null ? "null" : uprojectFile.FullName);
            return;
        }
        
        logger.LogInformation("Unreal Engine project detected: {uprojectFile}", uprojectFile);
    }
}