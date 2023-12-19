using System.CommandLine.IO;
using System.CommandLine.Rendering;
using Microsoft.Extensions.Logging;
using Ueco.Models;

namespace Ueco.Commands.Engine.List;

public static class ListCommand
{
    public static void Execute(ILogger logger)
    {
        logger.LogInformation("This is list command");
        var console = new SystemConsole();
        var enginesTableView = new EngineInstallsTableView(new []
        {
            new EngineAssociation
            {
                Name = "STO-Engine",
                Path = "C:\\Program Files\\Unreal Engine\\5.2\\Engine\\Binaries\\Win64\\UE4Editor.exe",
                Version = EngineVersion.UE5_2,
                IsDefault = false
            },
            new EngineAssociation
            {
                Name = "STO-Engine",
                Path = "C:\\Program Files\\Unreal Engine\\4.27\\Engine\\Binaries\\Win64\\UE4Editor.exe",
                Version = EngineVersion.UE5_3,
                IsDefault = true
            }
        });
        
        console.Append(enginesTableView);
    }
}