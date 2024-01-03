using UEScript.CLI.Commands;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services;

public interface IUnrealBuildToolService
{
    public Result<string, CommandError> Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null);
}