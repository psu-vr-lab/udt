using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Services;
using UEScript.Utils.Extensions;

namespace UEScript.CLI.Commands.Engine.Install;

public static class ConfigureInstallCommand
{
    public static void AddInstallCommand(this CliCommand command)
    {
        var installCommand = new CliCommand("install", "Install Unreal ENGINE from url")
        {
            new CliArgument<string>("name")
            {
                Description = "Name of the engine",
            },
            new CliArgument<FileInfo>("path")
            {
                Description = "Path to the engine",
            },
            new CliArgument<string>("url")
            {
                Description = "Url to the engine",
            },
            new CliOption<bool>("--isDefault")
            {
                Description = "Set this engine as default",
                Required = false,
                Aliases = { "-d" }
            },
        };

        installCommand.Action = CommandHandler.Create<string, FileInfo, string, bool, IHost>((
            name, 
            path,
            url,
            isDefault,
            host) =>
        {
            var serviceProvider = host.Services;

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("Engine.DeleteCommand");

            var engineAssociationRepository = serviceProvider.GetRequiredService<IUnrealEngineAssociationRepository>();
            var fileDownloader = serviceProvider.GetRequiredService<IFileDownloaderService>();

            var act = async () => await InstallCommand.Execute(name, path, isDefault, url, logger, fileDownloader, engineAssociationRepository);

            var result = Task.Run(act).Result;
            
            logger.LogResult(result);
        });

        command.Add(installCommand);
    }
}