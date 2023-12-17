using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Ueco.Commands.Build;

namespace Ueco.Commands;

public static class InjectCommands
{
    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        var rootCommand = new RootCommand("Unreal project manager");

        var fileArgument = new Argument<FileInfo>("file", "File to read");

        var buildCommand = new Command("build","Builds project");
        buildCommand.AddArgument(fileArgument);
        
        buildCommand.SetHandler((file) =>
        {
            using var scope = services.BuildServiceProvider();
            scope.GetRequiredService<BuildCommand>().Execute(file);
        }, fileArgument);
        
        rootCommand.AddCommand(buildCommand);

        services.AddSingleton(rootCommand);
        services.AddSingleton<BuildCommand>();
        
        return services;
    }
}