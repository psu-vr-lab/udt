using System.CommandLine.IO;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;
using UEScript.CLI.Services;

namespace UEScript.CLI.Commands.Engine.List;

public static class ListCommand
{
    public static Result<string, CommandError> Execute(IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        var console = new SystemConsole();
        var engines = engineAssociationRepository.GetUnrealEngines().ToArray();

        if (!engines.Any())
        {
            return CommandError.NoEngineAssociations();
        }
        
        var enginesTableView = new EngineInstallsTableView(engines);
        console.Append(enginesTableView);
        
        return Result<string, CommandError>.Ok(string.Empty);
    }
}