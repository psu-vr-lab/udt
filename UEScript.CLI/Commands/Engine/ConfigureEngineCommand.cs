using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Engine.Add;
using UEScript.CLI.Commands.Engine.Delete;
using UEScript.CLI.Commands.Engine.List;

namespace UEScript.CLI.Commands.Engine;

public static class ConfigureEngineCommand
{
    public static void AddEngineCommand(this CliRootCommand rootCommand)
    {
        var engineCommand = new CliCommand("engine", "Configure Unreal Engine installations");
        
        engineCommand.AddListCommand();
        engineCommand.AddAddCommand();
        engineCommand.AddDeleteCommand();
        
        engineCommand.Action = CommandHandler.Create<IHost>((host) =>
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("EngineCommand");
            
            EngineCommand.Execute(logger);
        });
        
        rootCommand.Add(engineCommand);
    }
}