using Microsoft.Extensions.Logging;

namespace UEScript.Logging;

public class UEScriptLoggerConfiguration
{
    public Dictionary<LogLevel, ConsoleColor> LogLevelColors { get; set; } 
        = new Dictionary<LogLevel, ConsoleColor>();
}
