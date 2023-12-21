using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UEScript.CLI.Common;
using UEScript.CLI.Models;

namespace UEScript.CLI.Services.Impl;

public class UnrealEngineEngineAssociationRepository : IUnrealEngineAssociationRepository
{
    private readonly HashSet<UnrealEngineAssociation> _unrealEngines;
    public string ConfigPath { get; }

    public UnrealEngineEngineAssociationRepository(IConfiguration configuration, ILogger<UnrealEngineAssociation> logger)
    {
        ConfigPath = configuration["ConfigPath"] ?? "ueco.json";
        if (!Path.IsPathRooted(ConfigPath))
        {
            ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigPath);
        }
        
        if (!File.Exists(ConfigPath))
        {
            logger.LogWarning("Config file not found: {0}", ConfigPath);
            logger.LogInformation("Creating new config file: {0}", ConfigPath);
            File.WriteAllText(ConfigPath, "[]");
        }
        
        var configContent = File.ReadAllText(ConfigPath ?? throw new Exception("ConfigPath is null"));
        try
        {
            _unrealEngines = JsonSerializer.Deserialize<HashSet<UnrealEngineAssociation>>(configContent, JsonSerializerStaticOptions.GetOptions()) ?? new HashSet<UnrealEngineAssociation>();
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Error while parsing config file: {0}", ConfigPath);
            logger.LogError(e.Message);
            _unrealEngines = new HashSet<UnrealEngineAssociation>();
            File.WriteAllText(ConfigPath, "[]");
        }
    }
    
    public IEnumerable<UnrealEngineAssociation> GetUnrealEngines()
    {
        return _unrealEngines;
    }

    public UnrealEngineAssociation GetUnrealEngine(int index)
    {
        return _unrealEngines.ElementAt(index);
    }

    public void AssociateUnrealEngine(UnrealEngineAssociation unrealEngine)
    {
        if (_unrealEngines.Contains(unrealEngine))
        {
            _unrealEngines.Remove(unrealEngine);
        }
        
        _unrealEngines.Add(unrealEngine);
        var json = JsonSerializer.Serialize(_unrealEngines, JsonSerializerStaticOptions.GetOptions());
        File.WriteAllText(ConfigPath, json);
    }

    public int GetUnrealEnginesCount()
    {
        return _unrealEngines.Count; 
    }

    public void DeleteUnrealEngine(int index)
    {
        _unrealEngines.Remove(_unrealEngines.ElementAt(index));
        var json = JsonSerializer.Serialize(_unrealEngines);
        File.WriteAllText(ConfigPath, json);
    }
    
    public UnrealEngineAssociation GetDefaultUnrealEngine()
    {
        if (!_unrealEngines.Any())
        {
            throw new Exception("No engines found");
        }
        
        return _unrealEngines.Count == 1 
            ? _unrealEngines.First() 
            : _unrealEngines.First(unrealEngineAssociation => unrealEngineAssociation.IsDefault);
    }
}