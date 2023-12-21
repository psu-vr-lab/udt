using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Common;
using UEScript.CLI.Models;

namespace UEScript.CLI.Services.Impl;

public class UnrealBuildToolService(IUnrealEngineAssociationRepository unrealEngineRepository, ILogger<UnrealBuildToolService> logger) : IUnrealBuildToolService
{
    public void Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null)
    {
        unrealEngine ??= unrealEngineRepository.GetDefaultUnrealEngine();
        
        var unrealBuildToolPath = UnrealPaths.GetUnrealBuildToolPath(unrealEngine);
        
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
        var buildArguments = "-waitmutex -NoHotReload -buildscw";
        
        logger.LogTrace($"Unreal Engine build command: {unrealBuildToolPath} {moduleName} {moduleTargets} {projectPath} {buildArguments}\n");

        var result = Process.Start(unrealBuildToolPath, $"{moduleName} {moduleTargets} {projectPath} {buildArguments}");
        result.WaitForExit();
    }
}