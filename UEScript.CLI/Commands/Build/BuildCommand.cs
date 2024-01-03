using Microsoft.Extensions.Logging;
using UEScript.CLI.Services;
using UEScript.Utils.Results;

namespace UEScript.CLI.Commands.Build;

public static class BuildCommand
{
    // 
    public static Result<string, CommandError> Execute(FileInfo file, IUnrealBuildToolService unrealBuildTool, ILogger logger)
    {
        logger.LogTrace("Build command start execution...");
        
        var uprojectFile = CommonCommandMethods.GetUprojectFile(file, logger);
        if (!uprojectFile.IsSuccess)
        {
            return Result<string, CommandError>.Error(uprojectFile);
        }
        
        // @Cleanup: if it last operation in this method, move this to return
        var result = unrealBuildTool.Build(uprojectFile);

        return result;
    }
}