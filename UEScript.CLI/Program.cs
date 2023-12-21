using System.CommandLine;
using System.CommandLine.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using UEScript.CLI.Commands;
using UEScript.CLI.Common;
using UEScript.CLI.Services;
using UEScript.CLI.Services.Impl;

namespace UEScript.CLI;

public static class  Program
{
    public static Task Main(string[] args)
    {
        var cliConfiguration = BuildCommandLine();
        cliConfiguration.UseHost(_ => Host.CreateDefaultBuilder().UseContentRoot(AppDomain.CurrentDomain.BaseDirectory),
                host =>
                {
                    host.ConfigureAppConfiguration((hostContext, config) =>
                        {
                            config
                                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json")
                                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                                .AddEnvironmentVariables();
                        });
                    
                    host.ConfigureLogging(builder =>
                    {
                        builder.ConfigureCustomFormatter();

                        var parseResult = cliConfiguration.Parse(args);
                        var debugLog = parseResult.GetValue<bool>("--debug-log");
                        if (debugLog)
                        {
                            builder.AddFilter("Microsoft.Hosting", LogLevel.Information);
                        }
                    });

                    host.ConfigureServices(services =>
                    {
                        services.AddLogging();
                        
                        services.AddSingleton<IUnrealEngineAssociationRepository, UnrealEngineEngineAssociationRepository>();
                        services.AddSingleton<IUnrealBuildToolService, UnrealBuildToolService>();
                        services.AddSingleton<IUnrealEngineEditorService, UnrealEngineEditorService>();
                    });
                });
        
        return cliConfiguration.InvokeAsync(args);
    }
    
    private static CliConfiguration BuildCommandLine()
    {
        return new CliConfiguration(ConfigureRootCommand.AddRootCommand());
    }

    private static void ConfigureCustomFormatter(this ILoggingBuilder builder)
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json")
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        const string color = "Logging:Console:FormatterOptions:Colors:";

        var traceColor = Enum.TryParse(config[color + "Trace"], out ConsoleColor parsedTraceColor);
        var informationColor = Enum.TryParse(config[color + "Information"], out ConsoleColor parsedInformationColor);
        var warningColor = Enum.TryParse(config[color + "Warning"], out ConsoleColor parsedWarningColor);
        var errorColor = Enum.TryParse(config[color + "Error"], out ConsoleColor parsedErrorColor);
        var debugColor = Enum.TryParse(config[color + "Debug"], out ConsoleColor parsedDebugColor);

        Enum.TryParse(config["Logging:Console:FormatterOptions:ColorBehavior"], out LoggerColorBehavior parsedColorBehavior);
        
        builder.AddCustomFormatter(options =>
        {
            options.ColorBehavior = parsedColorBehavior;
            options.TraceColor = traceColor ? parsedTraceColor : Console.ForegroundColor;
            options.WarningColor = warningColor ? parsedWarningColor : Console.ForegroundColor;
            options.ErrorColor = errorColor ? parsedErrorColor : Console.ForegroundColor;
            options.DebugColor = debugColor ? parsedDebugColor : Console.ForegroundColor;
            options.InformationColor = informationColor ? parsedInformationColor : Console.ForegroundColor;
            options.UseUtcTimestamp = config.GetValue<bool>("Logging:Console:FormatterOptions:UseUtcTimestamp");
        });
    }
}