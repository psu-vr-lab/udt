using System.CommandLine.IO;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;
using Ueco.Services;

namespace Ueco.Commands.Engine.List;

public static class ListCommand
{
    public static void Execute(IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        var console = new SystemConsole();
        var engines = engineAssociationRepository.GetUnrealEngines();

        if (!engines.Any())
        {
            logger.LogWarning("You don't have any engines installed. Use `ueco engine add` to add one.");
            return;
        }
        
        var enginesTableView = new EngineInstallsTableView(engines);
        console.Append(enginesTableView);
    }
}