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
        var engines = engineAssociationRepository.GetUnrealEngines().ToArray();

        if (engines.Length == 0)
        {
            return CommandError.NoEngineAssociations();
        }
        
        EngineInstallsTableView.ToTable(engines);
        
        return Result<string, CommandError>.Ok(string.Empty);
    }
}