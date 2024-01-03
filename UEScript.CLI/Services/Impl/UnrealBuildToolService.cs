using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands;
using UEScript.CLI.Common;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class UnrealBuildToolService(IUnrealEngineAssociationRepository unrealEngineRepository, ILogger<UnrealBuildToolService> logger) : IUnrealBuildToolService
{
    public Result<string, CommandError> Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null)
    {
        unrealEngine ??= unrealEngineRepository.GetDefaultUnrealEngine();
        
        var unrealBuildToolPath = UnrealPaths.GetUnrealEngineBuildToolPath(unrealEngine);
        
        var moduleName = Path.GetFileNameWithoutExtension(uprojectFile.Name) + "Editor";
        var moduleTargets = "Development";
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            moduleTargets = "Mac " + moduleTargets;
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            moduleTargets = "Win64 " + moduleTargets;
        }
        
        var projectPath = $"-project=\"{uprojectFile.FullName}\"";
        const string buildArguments = "-waitmutex -NoHotReload -buildscw";
        
        logger.LogInformation($"Unreal Engine build command: {unrealBuildToolPath} {moduleName} {moduleTargets} {projectPath} {buildArguments}");
        
        if (!File.Exists(unrealBuildToolPath))
        {
            return Result<string, CommandError>.Error(new CommandError($"Unreal Engine build tool path in config is set to ({unrealBuildToolPath}), but it doesn't exist"));
        }

        var result = Process.Start(unrealBuildToolPath, $"{moduleName} {moduleTargets} {projectPath} {buildArguments}");
        result.WaitForExit();
        
        return Result<string, CommandError>.Ok("Unreal Engine project was built");
    }
}