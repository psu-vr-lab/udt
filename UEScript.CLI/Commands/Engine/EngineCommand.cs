using Microsoft.Extensions.Logging;
using UEScript.CLI.Services;
using Result = UEScript.Utils.Results.Result<string, UEScript.CLI.Commands.CommandError>;

namespace UEScript.CLI.Commands.Engine;

public static class EngineCommand
{
    public static Result Execute(FileInfo file, IUnrealEngineEditorService unrealEngineEditorService, ILogger logger)
    {
        logger.LogTrace("Editor command start execution...");
        
        var uprojectFile = CommonCommandMethods.GetUprojectFile(file, logger);
        if (!uprojectFile.IsSuccess)
        {
            return Result.Error(uprojectFile);
        }
        
        unrealEngineEditorService.OpenUProject(uprojectFile);
        
        return Result.Ok("Unreal Editor was started");
    }
}