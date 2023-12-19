using Microsoft.Extensions.Logging;

namespace Ueco.Commands.Engine;

public static class EngineCommand
{
    public static void Execute(ILogger logger)
    {
        logger.LogInformation("This is the engine command");
    }
}