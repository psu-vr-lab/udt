using System.Diagnostics;
using UEScript.CLI.Common;
using UEScript.CLI.Models;

namespace UEScript.CLI.Services.Impl;

public class UnrealEngineEditorService(IUnrealEngineAssociationRepository unrealEngineRepository) : IUnrealEngineEditorService
{
    public void OpenUProject(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null)
    {
        unrealEngine ??= unrealEngineRepository.GetDefaultUnrealEngine();
        
        var unrealEditorPath = UnrealPaths.GetUnrealEngineEditorPath(unrealEngine);
        
        var result = Process.Start(unrealEditorPath, uprojectFile.FullName);
        result.WaitForExit();
    }
}