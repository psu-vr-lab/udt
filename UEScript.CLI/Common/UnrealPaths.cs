using System.Runtime.InteropServices;
using UEScript.CLI.Models;

namespace UEScript.CLI.Common;

public static class UnrealPaths
{
    public static string GetUnrealEngineBuildToolPath(UnrealEngineAssociation unrealEngine)
    {
        var path = unrealEngine.Path;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path += "/Engine/Build/BatchFiles/Mac/Build.sh";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path += "/Engine/Build/BatchFiles/Build.bat";
        }
        
        return path;
    }

    public static string GetUnrealEngineEditorPath(UnrealEngineAssociation unrealEngine)
    {
        var path = unrealEngine.Path;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            path += "/Engine/Binaries/Mac/UnrealEditor.app/Contents/MacOS/UnrealEditor";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            path += "/Engine/Binaries/Win64/UnrealEditor.exe";
        }
        
        return path;
    }
}