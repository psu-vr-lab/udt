using UEScript.CLI.Commands;
using UEScript.CLI.Common;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class UnrealArgsBuilder(IUnrealEngineAssociationRepository unrealEngineAssociationRepository) : IUnrealArgsBuilder
{
    public string BuildArguments(params BuildArgument[] arguments)
    {
        // @Incomplete: Arguments rewrite
        return arguments.Aggregate(string.Empty, (current, arg) => current + arg switch
        {
            BuildArgument.WaitMutex => " -waitmutex ",
            BuildArgument.NoHotReload => "-NoHotReload ",
            BuildArgument.BuildScw => "-buildscw ",
            _ => throw new ArgumentOutOfRangeException()
        });
    }
    
    public Result<(string UnrealBuildTool, string Args), CommandError> UnrealBuildToolArgs(FileInfo uprojectFile, ModuleTarget moduleTarget, UnrealEngineAssociation? unrealEngine = null)
    {
        var unrealBuildToolPath = GetUnrealBuildToolPath(unrealEngine);
        
        if (!File.Exists(unrealBuildToolPath))
        {
            return TypedCommmandErrors.InvalidPathToBuildTool(unrealBuildToolPath);
        }
        
        var moduleName = Path.GetFileNameWithoutExtension(uprojectFile.Name) + "Editor";
        var moduleTargets = PlatformNames.GetPlatformName() + moduleTarget;
        
        // @Incomplete: ProjectPath is also arguments...
        var projectPath = $"-project=\"{uprojectFile.FullName}\"";
        var buildArgs = BuildArguments(BuildArgument.WaitMutex, BuildArgument.NoHotReload, BuildArgument.BuildScw);

        return (unrealBuildToolPath, $"{moduleName} {moduleTargets} {projectPath} {buildArgs}");
    }

    private string GetUnrealBuildToolPath(UnrealEngineAssociation? unrealEngine)
    {
        return UnrealPaths.GetUnrealEngineBuildToolPath(
            unrealEngine ?? unrealEngineAssociationRepository.GetDefaultUnrealEngine());
    }
}