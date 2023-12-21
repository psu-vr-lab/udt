using Microsoft.Extensions.Logging;
using UEScript.CLI.Services;
using Result = UEScript.Utils.Results.Result<string, UEScript.CLI.Commands.CommandError>;

namespace UEScript.CLI.Commands.Build;

public static class BuildCommand
{
    public static Result Execute(FileInfo file, IUnrealBuildToolService unrealBuildTool, ILogger logger)
    {
        logger.LogTrace("Build command start execution...");
        
        var uprojectFile = CommonCommandMethods.GetUprojectFile(file, logger);
        if (!uprojectFile.IsSuccess)
        {
            return Result.Error(uprojectFile);
        }
        
        unrealBuildTool.Build(uprojectFile);
        
        return Result.Ok("Unreal Engine project was built");
    }
}