namespace Ueco.Models;

public enum EngineVersion
{
    UE_NONE,
    UE_5_2,
    UE_5_3,
}

public static class EngineVersionExtensions
{
    public static EngineVersion Parse(string value)
    {
        return value switch
        {
            "UE_5.2" => EngineVersion.UE_5_2,
            "UE_5.3" => EngineVersion.UE_5_3,
            _ => EngineVersion.UE_NONE
        };
    }
}