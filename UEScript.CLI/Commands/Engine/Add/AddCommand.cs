using System.Text.Json;
using Microsoft.Extensions.Logging;
using UEScript.Utils.Results;
using UEScript.CLI.Common;
using UEScript.CLI.Models;
using UEScript.CLI.Services;

namespace UEScript.CLI.Commands.Engine.Add;

public static class AddCommand
{
    public static Result<string, CommandError> Execute(string name, FileInfo path, bool isDefault, IUnrealEngineAssociationRepository engineAssociationRepository, ILogger logger)
    {
        var ueDir = path.Directory;
        
        logger.LogTrace("Add command start execution...");

        if (ueDir is null || !ueDir.Exists)
        {
            return CommandError.PathIsNotDirectory(path.FullName);
        }
        
        if (!ueDir.Name.StartsWith("UE_"))
        {
            var findUeDir = path.Directory?.GetDirectories().FirstOrDefault(dir => dir.Name.StartsWith("UE_"));
            if (findUeDir is null)
            {
                return CommandError.DirectoryHasWrongName(ueDir.FullName);
            }
            
            ueDir = findUeDir;
        }
        
        var unrealEngineVersion = ueDir.Name;
        logger.LogTrace("Unreal Engine {unrealEngineVersion} directory detected: {ueDir}", unrealEngineVersion, ueDir );

        var engineAssociation = new UnrealEngineAssociation
        {
            Name = name,
            Path = ueDir.FullName,
            Version = unrealEngineVersion.ToUnrealEngineVersion(),
            IsDefault = isDefault
        };
        
        logger.LogInformation("Engine association created: ");
        logger.LogTrace(JsonSerializer.Serialize(engineAssociation, JsonSerializerStaticOptions.GetOptions()));
        engineAssociationRepository.AssociateUnrealEngine(engineAssociation);
        
        return Result<string, CommandError>.Ok($"Engine association added to config file: {engineAssociationRepository.ConfigPath}");
    }
}