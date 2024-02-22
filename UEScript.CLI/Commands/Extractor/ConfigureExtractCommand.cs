using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine.NamingConventionBinder;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Commands.Build;
using UEScript.CLI.Services;
using UEScript.Utils.Extensions;
using static System.CommandLine.Help.HelpBuilder;
using System.Xml.Linq;
using UEScript.CLI.Commands.Engine.Add;

namespace UEScript.CLI.Commands.ArchiveExctractor;

public static class ConfigureExtractCommand
{
    public static void AddExtractCommand(this CliRootCommand rootCommand)
    {
        var extractCommand = new CliCommand("extract", "Exctracts archive to the destination path")
        {

            new CliArgument<FileInfo>("file")
            {
                Description = "Archive to extract",
            },
            new CliArgument<FileInfo>("destination")
            {
                Description = "Destination path",
            },
        };

        extractCommand.Action = CommandHandler.Create<FileInfo, FileInfo, IHost>((file, destination, host) =>
        {
            var serviceProvider = host.Services;
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("ExtractCommand");

            var archiveExtractor = serviceProvider.GetRequiredService<IArchiveExtractor>();
            var result = ExtractCommand.Execute(file, destination, archiveExtractor, logger);
            logger.LogResult(result);
        });

        rootCommand.Add(extractCommand);
    }
}