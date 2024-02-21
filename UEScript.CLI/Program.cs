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
using System.Reflection;
using UEScript.Logging;

[assembly: AssemblyVersion("3.1.*")]

namespace UEScript.CLI;

public static class  Program
{
    public static Task Main(string[] args)
    {
        var cliConfiguration = BuildCommandLine();
        cliConfiguration.UseHost(_ => Host.CreateDefaultBuilder().UseContentRoot(AppDomain.CurrentDomain.BaseDirectory),
                host =>
                {
                    host.ConfigureAppConfiguration(config =>
                        {
                            config
                                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json")
                                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                                .AddEnvironmentVariables();
                        });
                    
                    host.ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddUEScriptLogger(configure =>
                        {
                            // @Cleanup: Check how to do this stuff automatically
                            var configuration = GetConfiguration();
                            var logLevelColors = configuration.GetSection("Logging:UEScript:LogLevelColors").Get<Dictionary<LogLevel, ConsoleColor>>();
                            if (logLevelColors is null)
                            {
                                // Fallback to default values
                                // @Cleanup: Move it just to UEScriptLoggerConfiguration default value
                                logLevelColors = new Dictionary<LogLevel, ConsoleColor>()
                                {
                                    {
                                        LogLevel.Trace, ConsoleColor.Gray
                                    },
                                    {
                                        LogLevel.Debug, ConsoleColor.Gray
                                    },
                                    {
                                        LogLevel.Information, ConsoleColor.DarkGreen
                                    },
                                    {
                                        LogLevel.Warning, ConsoleColor.Cyan
                                    },
                                    {
                                        LogLevel.Error, ConsoleColor.Red
                                    },
                                    {
                                        LogLevel.Critical, ConsoleColor.Red
                                    }
                                };
                                
                                configure.LogLevelColors = logLevelColors;
                            }
                        });
                        var parseResult = cliConfiguration.Parse(args);
                        var debugLog = parseResult.GetValue<bool>("--debug-log");
                        if (debugLog)
                        {
                            builder.AddFilter("Microsoft.Hosting", LogLevel.Information);
                        }
                    });

                    host.ConfigureServices(services =>
                    {
                        services.AddSingleton<IUnrealEngineAssociationRepository, UnrealEngineEngineAssociationRepository>();
                        services.AddSingleton<IUnrealBuildToolService, UnrealBuildToolService>();
                        services.AddSingleton<IUnrealEngineEditorService, UnrealEngineEditorService>();
                        services.AddSingleton<IFileDownloaderService, FileDownloaderServiceService>();
                    });
                });
        
        return cliConfiguration.InvokeAsync(args);
    }
    
    private static CliConfiguration BuildCommandLine()
    {
        return new CliConfiguration(ConfigureRootCommand.AddRootCommand());
    }

    private static IConfiguration GetConfiguration()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json")
            .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
        
        return config;
    }
}