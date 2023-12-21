using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Commands.Engine.Delete;
using UEScript.CLI.Commands.Engine.List;
using UEScript.CLI.Common;
using UEScript.CLI.Services;

namespace UEScript.CLI.Commands.Engine;

public static class ConfigureEngineCommand
{
    public static void AddEngineCommand(this CliRootCommand rootCommand)
    {
        var engineCommand = new CliCommand("engine", "Manage installed engines")
        {
            new CliArgument<FileInfo>("file")
            {
                Description = "Path to uproject file",
            }
        };
        
        engineCommand.AddListCommand();
        engineCommand.AddAddCommand();
        engineCommand.AddDeleteCommand();
        
        engineCommand.Action = CommandHandler.Create<FileInfo, IHost>((file, host) =>
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("EngineCommand");
            
            var unrealEngineEditorService = serviceProvider.GetRequiredService<IUnrealEngineEditorService>();
            
            var result = EngineCommand.Execute(file, unrealEngineEditorService, logger);
            logger.LogResult(result);
        });
        
        rootCommand.Add(engineCommand);
    }
}