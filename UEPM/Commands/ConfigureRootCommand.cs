using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Ueco.Commands.Build;

namespace Ueco.Commands;

public static class ConfigureRootCommand
{
    public static RootCommand AddRootCommand(IConfiguration configuration)
    {
        var rootCommand = new RootCommand(configuration["Root:Description"] ?? "Unreal Engine Command Line Interface");

        var commandsConfiguration = configuration.GetSection("Root:Commands").Get<string[]>();

        foreach (var command in commandsConfiguration ?? new []{ "build" })
        {
            switch (command)
            {
                case "build":
                    rootCommand.AddBuildCommand(configuration);
                    break;
            }
        }
        
        return rootCommand;
    }
}