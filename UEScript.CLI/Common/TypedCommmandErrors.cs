using UEScript.CLI.Commands;

namespace UEScript.CLI.Common;

// @Cleanup: Create normal Typed error classes and handle them
public static class TypedCommmandErrors
{
    public static CommandError InvalidPathToBuildTool(string unrealBuildToolPath)
    {
        return new CommandError($"Unreal Engine build tool path in config is set to ({unrealBuildToolPath}), but it doesn't exist");
    }
}