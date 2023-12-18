using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Ueco.Commands.Binders;

namespace Ueco.Commands.Build;

public static class ConfigureBuildCommand
{
    public static void AddBuildCommand(this RootCommand rootCommand, IConfiguration configuration)
    {
        var buildCommand = new Command(
            configuration["Build:Name"] ?? "build", 
            configuration["Build:Description"] ?? "Builds the project");

        var fileArgument = new Argument<FileInfo>("file", "File to read");
        buildCommand.AddArgument(fileArgument);
        
        buildCommand.SetHandler(async (file, conf) =>
        {
            await BuildCommand.ExecuteAsync(file, conf);
        }, 
            fileArgument, 
            new AppConfigurationBinder());
        
        rootCommand.AddCommand(buildCommand);
    }
}