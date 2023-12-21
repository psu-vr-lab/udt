using UEScript.CLI.Models;

namespace UEScript.CLI.Services;

public interface IUnrealEngineEditorService
{
    public void OpenUProject(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null);
}