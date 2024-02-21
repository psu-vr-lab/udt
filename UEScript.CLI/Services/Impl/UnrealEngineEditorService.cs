using System.Diagnostics;
using System.Runtime.InteropServices;
using UEScript.CLI.Common;
using UEScript.CLI.Models;

namespace UEScript.CLI.Services.Impl;

public class UnrealEngineEditorService(IUnrealEngineAssociationRepository unrealEngineRepository) : IUnrealEngineEditorService
{
    public void OpenUProject(FileInfo uprojectFile, UnrealEngineAssociation? unrealEngine = null)
    {
        unrealEngine ??= unrealEngineRepository.GetDefaultUnrealEngine();
        
        var unrealEditorPath = UnrealPaths.GetUnrealEngineEditorPath(unrealEngine);

        var result = default(Process);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            result = Process.Start(unrealEditorPath, uprojectFile.FullName);
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            result = Process.Start("open", $"-n {unrealEditorPath} --args {uprojectFile.FullName}");
        }

        result?.WaitForExit();
    }
}