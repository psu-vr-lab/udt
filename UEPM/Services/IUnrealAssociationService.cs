using Ueco.Models;

namespace Ueco.Services;

public interface IUnrealAssociationService
{
    public string GetUATByVersion(EngineVersion version);
    public string GetUATByPath(string path);
}