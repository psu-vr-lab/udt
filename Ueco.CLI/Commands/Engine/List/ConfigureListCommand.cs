using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ueco.Services;

namespace Ueco.Commands.Engine.List;

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
           
            ListCommand.Execute(unrealEngineAssociationRepository, logger);
        });
        
        command.Add(listCommand);
    }
}