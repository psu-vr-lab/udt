namespace UEScript.CLI.Models;

public enum UnrealEngineVersion
{
    UeNone,
    Ue52,
    Ue53,
}

public static class UnrealEngineVersionExtensions
{
    public static UnrealEngineVersion ToUnrealEngineVersion(this string version)
    {
        return version switch
        {
            "UE_5.2" => UnrealEngineVersion.Ue52,
            "UE_5.3" => UnrealEngineVersion.Ue53,
            _ => UnrealEngineVersion.UeNone
        };
    }
}