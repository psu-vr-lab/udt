using System.CommandLine;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.EventLog;
using Ueco.Commands;
using Ueco.Common;

namespace Ueco;

// TODO: Revert back some DI. Find out way to do inject services

public static class  Program
{
    private static Task Main(string[] args)
    {
        var cliConfiguration = BuildCommandLine();
        cliConfiguration.UseHost(_ => Host.CreateDefaultBuilder(),
                host =>
                {
                    host.ConfigureServices(services => { services.AddSingleton<IGreeter, Greeter>(); });

                    host.ConfigureLogging(builder =>
                    {
                        // builder.AddSimpleConsole(options =>
                        // {
                        //     options.SingleLine = true;
                        //     options.ColorBehavior = LoggerColorBehavior.Enabled;
                        // });

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
        var root = ConfigureRootCommand.AddRootCommand();
        return new CliConfiguration(root);
    }
    
    private static void Run(GreeterOptions options, IHost host)
    {
        var serviceProvider = host.Services;
        var greeter = serviceProvider.GetRequiredService<IGreeter>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(Program));

        var name = options.Name;
        logger.LogInformation(HostingPlaygroundLogEvents.GreetEvent, "Greeting was requested for: {name}", name);
        greeter.Greet(name);
    }
    
    // private static async Task<int> Main(string[] args)
    // {
    //     var serviceCollection = new ServiceCollection();
    //     serviceCollection.AddJsonCommandSettings();
    //
    //     var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>();
    //     Debug.Assert(configuration is not null, "configuration is null");
    //     
    //     var rootCommand = ConfigureRootCommand.AddRootCommand(configuration);
    //     return await rootCommand.InvokeAsync(args);
    // }
}