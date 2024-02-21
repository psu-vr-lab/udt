using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Services;
using UEScript.Utils.Extensions;

namespace UEScript.CLI.Commands.Engine.Add;

public static class ConfigureAddCommand
{
    public static void AddAddCommand(this CliCommand command)
    {
        var addCommand = new CliCommand("add", "Add unreal engine install")
        {
            new CliArgument<string>("name")
            {
                Description = "Name of the engine",
            },
            new CliArgument<FileInfo>("path")
            {
                Description = "Path to the engine",
            },
            new CliOption<bool>("--isDefault")
            {
                Description = "Set this engine as default",
                Required = false,
                Aliases = { "-d" }
            }
        };
        
        addCommand.Action = CommandHandler.Create<string, bool, FileInfo, IHost>((name, isDefault, path, host) =>
        {
            var serviceProvider = host.Services;
            
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Engine.ListCommand");
            
            var unrealEngineAssociationRepository = serviceProvider.GetRequiredService<IUnrealEngineAssociationRepository>();
           
            var result = AddCommand.Execute(name, path, isDefault, unrealEngineAssociationRepository, logger);
            logger.LogResult(result);
        });
        
        command.Add(addCommand);
    }
}