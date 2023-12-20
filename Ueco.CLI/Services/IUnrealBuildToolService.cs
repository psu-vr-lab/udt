using Ueco.Models;

namespace Ueco.Services;

public interface IUnrealBuildToolService
{
    public void Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null);
}