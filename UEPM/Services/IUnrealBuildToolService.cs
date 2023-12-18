namespace Ueco.Services;

public interface IUnrealBuildToolService
{
    public Task Build(string projectPath, params string[] args);
}