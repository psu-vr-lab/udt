using System.CommandLine;
using Microsoft.Extensions.Configuration;
using Ueco.Commands.Build;

namespace Ueco.Commands;

public static class ConfigureRootCommand
{
    public static CliRootCommand AddRootCommand()
    {
        var rootCommand = new CliRootCommand("Unreal Engine Command Line Interface")
        {
            new CliOption<bool>("--debug-log")
            {
                Description = "Enable debug logging",
                Hidden = true
            }
        };

        rootCommand.AddBuildCommand();
        
        return rootCommand;
    }
}