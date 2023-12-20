using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Ueco.Common;
using Ueco.Models;
using Ueco.Services;

namespace Ueco.Commands.Engine.Add;

public static class AddCommand
{
    public static void Execute(string name, FileInfo path, bool isDefault, IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        var ueDir = path.Directory;
        
        logger.LogTrace("Add command start execution...");

        if (ueDir is null)
        {
            logger.LogError("Path is not a directory: {0}", path.FullName);
            return;
        }

        if (!ueDir.Name.StartsWith("UE_"))
        {
            var findUeDir = path.Directory.GetDirectories().FirstOrDefault(dir => dir.Name.StartsWith("UE_"));

            if (findUeDir is null)
            {
                logger.LogError("Path is not a containing or starting with UE_(5.2,5.3,...) directory: {0}", path.FullName);
                return;
            }
            
            ueDir = findUeDir;
        }
        
        var unrealEngineVersion = ueDir.Name;
        logger.LogInformation("Unreal Engine {unrealEngineVersion} directory detected: {ueDir}", unrealEngineVersion, ueDir );

        var engineAssociation = new UnrealEngineAssociation
        {
            Name = name,
            Path = ueDir.FullName,
            Version = EngineVersionExtensions.Parse(unrealEngineVersion),
            IsDefault = isDefault
        };
        
        logger.LogInformation("Engine association created: ");
        logger.LogTrace(JsonSerializer.Serialize(engineAssociation, JsonSerializerStaticOptions.GetOptions()));
    }
}