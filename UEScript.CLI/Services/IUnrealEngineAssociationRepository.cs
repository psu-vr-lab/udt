using UEScript.CLI.Models;

namespace UEScript.CLI.Services;

public interface IUnrealEngineAssociationRepository
{
    public IEnumerable<UnrealEngineAssociation> GetUnrealEngines();
    public UnrealEngineAssociation GetUnrealEngine(int index);
    public void AssociateUnrealEngine(UnrealEngineAssociation unrealEngine);
    public int GetUnrealEnginesCount();
    public void DeleteUnrealEngine(int index);
    public UnrealEngineAssociation GetDefaultUnrealEngine();
    public string ConfigPath { get; }
}