using Ueco.Models;

namespace Ueco.Services;

public interface IUnrealEngineAssociationRepository
{
    public List<UnrealEngineAssociation> GetUnrealEngines();
    public UnrealEngineAssociation GetUnrealEngine(int index);
    public void AddUnrealEngine(UnrealEngineAssociation unrealEngine);
    public void DeleteUnrealEngine(int index);
}