using System.CommandLine.IO;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;
using Ueco.Services;
using Ueco.Utils.Results;

namespace Ueco.Commands.Engine.List;

public static class ListCommand
{
    public static Result<string, ListCommandError> Execute(IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        var console = new SystemConsole();
        var engines = engineAssociationRepository.GetUnrealEngines().ToArray();

        if (!engines.Any())
        {
            return ListCommandError.NoEngineAssociations();
        }
        
        var enginesTableView = new EngineInstallsTableView(engines);
        console.Append(enginesTableView);
        
        return Result<string, ListCommandError>.Ok(string.Empty);
    }
}