using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;
using UEScript.CLI.Services;

namespace UEScript.CLI.Commands.Engine.Delete;

public static class DeleteCommand
{
    public static Result<string, CommandError> Execute(int index, IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        logger.LogTrace("Delete command start execution...");

        index -= 1;

        if (index < 0 || index >= engineAssociationRepository.GetUnrealEnginesCount())
        {
            return new CommandError($"Index out of range: {index + 1}, max {engineAssociationRepository.GetUnrealEnginesCount()}");
        }
        
        var unrealEngineName = engineAssociationRepository.GetUnrealEngine(index).Name;
        logger.LogInformation("Deleting {unrealEngineName} Unreal Engine install...", unrealEngineName);
        
        engineAssociationRepository.DeleteUnrealEngine(index);
        
        return Result<string, CommandError>.Ok($"{unrealEngineName}:{index + 1} Unreal Engine deleted");
    }
}