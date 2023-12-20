using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ueco.Common;
using Ueco.Models;

namespace Ueco.Services.Impl;

public class UnrealEngineEngineAssociationRepository : IUnrealEngineAssociationRepository
{
    private readonly List<UnrealEngineAssociation> _unrealEngines;
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
            _unrealEngines = JsonSerializer.Deserialize<List<UnrealEngineAssociation>>(configContent, JsonSerializerStaticOptions.GetOptions()) ?? new List<UnrealEngineAssociation>();
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Error while parsing config file: {0}", ConfigPath);
            logger.LogError(e.Message);
            _unrealEngines = new List<UnrealEngineAssociation>();
            File.WriteAllText(ConfigPath, "[]");
        }
        
        // TODO: use HashSet
        var dublicates = _unrealEngines.GroupBy(unrealEngine => unrealEngine.Name)
            .Where(g => g.Count() > 1)
            .Select(y => y.Key)
            .ToList();

        if (dublicates.Any())
        {
            logger.LogWarning("Config file contains dublicate engines: {0}", string.Join(", ", dublicates));
            foreach (var dublicate in dublicates)
            {
                _unrealEngines.RemoveAll(unrealEngine => unrealEngine.Name == dublicate);
                logger.LogWarning("Removed dublicate engine: {0}", dublicate);
            }
            
            var json = JsonSerializer.Serialize(_unrealEngines, JsonSerializerStaticOptions.GetOptions());
            File.WriteAllText(ConfigPath, json);
        }
    }
    
    public List<UnrealEngineAssociation> GetUnrealEngines()
    {
        return _unrealEngines;
    }

    public UnrealEngineAssociation GetUnrealEngine(int index)
    {
        return _unrealEngines[index];
    }

    public void AssociateUnrealEngine(UnrealEngineAssociation unrealEngine)
    {
        if (_unrealEngines.Any(engine => engine.Name == unrealEngine.Name))
        {
            _unrealEngines.RemoveAll(engine => engine.Name == unrealEngine.Name);
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
        _unrealEngines.RemoveAt(index);
        var json = JsonSerializer.Serialize(_unrealEngines);
        File.WriteAllText(ConfigPath, json);
    }
}