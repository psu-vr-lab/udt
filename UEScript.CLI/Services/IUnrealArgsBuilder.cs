using UEScript.CLI.Commands;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IUnrealArgsBuilder
{
    public Result<(string UnrealBuildTool, string Args), CommandError> UnrealBuildToolArgs(
        FileInfo uprojectFile, 
        ModuleTarget moduleTarget, 
        UnrealEngineAssociation? unrealEngine = null);
}