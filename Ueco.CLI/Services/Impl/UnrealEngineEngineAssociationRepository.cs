using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ueco.Models;

namespace Ueco.Services.Impl;

public class UnrealEngineEngineAssociationRepository : IUnrealEngineAssociationRepository
{
    private readonly List<UnrealEngineAssociation> _unrealEngines;
    private readonly string _configPath;

    public UnrealEngineEngineAssociationRepository(IConfiguration configuration, ILogger<UnrealEngineAssociation> logger)
    {
        _configPath = configuration["ConfigPath"] ?? "ueco.json";
        if (!Path.IsPathRooted(_configPath))
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configPath);
        }
        
        if (!File.Exists(_configPath))
        {
            logger.LogWarning("Config file not found: {0}", _configPath);
            logger.LogInformation("Creating new config file: {0}", _configPath);
            File.WriteAllText(_configPath, "[]");
        }
        
        var configContent = File.ReadAllText(_configPath ?? throw new Exception("ConfigPath is null"));
        _unrealEngines = JsonSerializer.Deserialize<List<UnrealEngineAssociation>>(configContent) ?? new List<UnrealEngineAssociation>();
    }
    
    public List<UnrealEngineAssociation> GetUnrealEngines()
    {
        return _unrealEngines;
    }

    public UnrealEngineAssociation GetUnrealEngine(int index)
    {
        return _unrealEngines[index];
    }

    public void AddUnrealEngine(UnrealEngineAssociation unrealEngine)
    {
        _unrealEngines.Add(unrealEngine);
        var json = JsonSerializer.Serialize(_unrealEngines);
        File.WriteAllText(_configPath, json);
    }

    public void DeleteUnrealEngine(int index)
    {
        _unrealEngines.RemoveAt(index);
        var json = JsonSerializer.Serialize(_unrealEngines);
        File.WriteAllText(_configPath, json);
    }
}