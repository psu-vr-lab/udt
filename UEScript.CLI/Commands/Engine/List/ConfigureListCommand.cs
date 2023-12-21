using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Common;
using UEScript.CLI.Services;

namespace UEScript.CLI.Commands.Engine.List;

public static class ConfigureListCommand
{
    public static void AddListCommand(this CliCommand command)
    {
        var listCommand = new CliCommand("list", "List installed Unreal Engine versions");
        
        listCommand.Action = CommandHandler.Create<IHost>((host) =>
        {
            var serviceProvider = host.Services;
            
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Engine.ListCommand");
            
            var unrealEngineAssociationRepository = serviceProvider.GetRequiredService<IUnrealEngineAssociationRepository>();
           
            var result = ListCommand.Execute(unrealEngineAssociationRepository, logger);
            logger.LogResult(result);
        });
        
        command.Add(listCommand);
    }
}