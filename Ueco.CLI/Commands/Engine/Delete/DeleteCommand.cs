using Microsoft.Extensions.Logging;
using Ueco.Services;
using Ueco.Utils.Results;

namespace Ueco.Commands.Engine.Delete;

public static class DeleteCommand
{
    public static Result<string, DeleteCommandError> Execute(int index, IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        logger.LogTrace("Delete command start execution...");

        index -= 1;

        if (index < 0 || index >= engineAssociationRepository.GetUnrealEnginesCount())
        {
            return DeleteCommandError.IndexOutOfRange(index + 1, engineAssociationRepository.GetUnrealEnginesCount());
        }
        
        var unrealEngineName = engineAssociationRepository.GetUnrealEngine(index).Name;
        logger.LogInformation("Deleting {unrealEngineName} Unreal Engine install...", unrealEngineName);
        
        engineAssociationRepository.DeleteUnrealEngine(index);
        
        return Result<string, DeleteCommandError>.Ok($"{unrealEngineName}:{index + 1} Unreal Engine deleted");
    }
}