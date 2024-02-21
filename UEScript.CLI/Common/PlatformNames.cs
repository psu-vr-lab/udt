using System.Runtime.InteropServices;

namespace UEScript.CLI.Common;

public static class PlatformNames
{
    public const string Windows = "Win64";
    public const string Mac = "Mac";

    public static string GetPlatformName()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Windows;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Mac;
        }

        return "Unknown";
    }
}