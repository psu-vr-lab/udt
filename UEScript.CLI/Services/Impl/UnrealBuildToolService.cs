using System.Diagnostics;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands;
using UEScript.CLI.Models;
using UEScript.Utils.Results;

namespace UEScript.CLI.Services.Impl;

public class UnrealBuildToolService(
    IUnrealArgsBuilder unrealArgsBuilder,
    ILogger<UnrealBuildToolService> logger) : IUnrealBuildToolService
{
    public Result<string, CommandError> Build(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null)
    {
        // @Cleanup: Handle ModelTarget
        var unrealArgsResult = unrealArgsBuilder.UnrealBuildToolArgs(uprojectFile, ModuleTarget.Development, unrealEngine);

        if (!unrealArgsResult.IsSuccess)
        {
            return unrealArgsResult.GetError()!;
        }

        var unrealArgs = unrealArgsResult.GetValue();
        
        logger.LogInformation($"Unreal Engine build command: {unrealArgs}");
        
        var result = Process.Start(unrealArgs.UnrealBuildTool, unrealArgs.Args);
        result.WaitForExit();
        
        return Result<string, CommandError>.Ok("Unreal Engine project was built");
    }
}