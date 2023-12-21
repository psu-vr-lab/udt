using UEScript.CLI.Models;

namespace UEScript.CLI.Services;

public interface IUnrealBuildToolService
{
    public void Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null);
}