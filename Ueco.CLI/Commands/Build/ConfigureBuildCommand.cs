using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ueco.Common;
using Ueco.Utils.Extensions;

namespace Ueco.Commands.Build;

public static class ConfigureBuildCommand
{
    public static void AddBuildCommand(this CliRootCommand rootCommand)
    {
        var buildCommand = new CliCommand("build", "Build unreal engine project")
        {
            new CliArgument<FileInfo>("file")
            {
                Description = "File to build",
            }
        };
        
        buildCommand.Action = CommandHandler.Create<FileInfo, IHost>((file, host) =>
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("BuildCommand");
            
            var result = BuildCommand.Execute(file, logger);
            logger.LogResult(result);
        });
        
        rootCommand.Add(buildCommand);
    }
}