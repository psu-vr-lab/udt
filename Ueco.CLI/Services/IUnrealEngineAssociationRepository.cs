using Ueco.Models;

namespace Ueco.Services;

public interface IUnrealEngineAssociationRepository
{
    public List<UnrealEngineAssociation> GetUnrealEngines();
    public UnrealEngineAssociation GetUnrealEngine(int index);
    public void AssociateUnrealEngine(UnrealEngineAssociation unrealEngine);
    public int GetUnrealEnginesCount();
    public void DeleteUnrealEngine(int index);
    public string ConfigPath { get; }
}