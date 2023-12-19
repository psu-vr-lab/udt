using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            
            ListCommand.Execute(logger);
        });
        
        command.Add(listCommand);
    }
}