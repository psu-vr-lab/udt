using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Ueco.Commands;
using Ueco.Common;

namespace Ueco;

public static class  Program
{
    private static Task Main(string[] args)
    {
        var cliConfiguration = BuildCommandLine();
        cliConfiguration.UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureLogging(builder =>
                    {
                        builder.AddCustomFormatter(options =>
                        {
                            options.ColorBehavior = LoggerColorBehavior.Enabled;
                            options.TraceColor = ConsoleColor.White;
                            options.WarningColor = ConsoleColor.Yellow;
                            options.ErrorColor = ConsoleColor.Red;
                            options.DebugColor = ConsoleColor.DarkCyan;
                            options.InformationColor = ConsoleColor.Green;
                            options.UseUtcTimestamp = false;
                        });
                        
                        var parseResult = cliConfiguration.Parse(args);
                        var debugLog = parseResult.GetValue<bool>("--debug-log");
                        if (debugLog)
                        {
                            builder.AddFilter("Microsoft.Hosting", LogLevel.Information);
                        }
                    });
                });

        return cliConfiguration.InvokeAsync(args);
    }

    private static CliConfiguration BuildCommandLine()
    {
        return new CliConfiguration(ConfigureRootCommand.AddRootCommand());
    }
}