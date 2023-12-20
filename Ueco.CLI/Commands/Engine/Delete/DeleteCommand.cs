using Microsoft.Extensions.Logging;
using Ueco.Services;

namespace Ueco.Commands.Engine.Delete;

public static class DeleteCommand
{
    public static void Execute(int index, IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        logger.LogTrace("Delete command start execution...");

        index = index - 1;

        if (index < 0 || index >= engineAssociationRepository.GetUnrealEnginesCount())
        {
            logger.LogError("Index out of range: {index}", index + 1);
            return;
        }
        
        engineAssociationRepository.DeleteUnrealEngine(index);
        logger.LogInformation("Unreal Engine {index} deleted", index + 1);
    }
}