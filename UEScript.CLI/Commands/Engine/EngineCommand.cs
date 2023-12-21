using Microsoft.Extensions.Logging;

namespace UEScript.CLI.Commands.Engine;

public static class EngineCommand
{
    public static void Execute(ILogger logger)
    {
        logger.LogInformation("This is the engine command");
    }
}