using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ueco.Common;
using Ueco.Services;

namespace Ueco.Commands.Engine.Delete;

public static class ConfigureDeleteCommand
{
    public static void AddDeleteCommand(this CliCommand command)
    {
        var deleteCommand = new CliCommand("delete", "Delete unreal engine install")
        {
            new CliArgument<int>("index")
            {
                Description = "Index of the engine",
            }
        };

        deleteCommand.Action = CommandHandler.Create<int, IHost>((index, host) =>
        {
            var serviceProvider = host.Services;
            
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Engine.DeleteCommand");
            
            var engineAssociationRepository = serviceProvider.GetRequiredService<IUnrealEngineAssociationRepository>();
            
            var result = DeleteCommand.Execute(index, engineAssociationRepository, logger);
            logger.LogResult(result);
        });
        
        command.Add(deleteCommand);
    }
}