using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace UEScript.CLI.Common.Console;

public class UEScriptConsoleFormatterOptions : ConsoleFormatterOptions
{
    public LoggerColorBehavior ColorBehavior { get; set; }
    
    public ConsoleColor WarningColor { get; set; }
    public ConsoleColor TraceColor { get; set; }
    public ConsoleColor ErrorColor { get; set; }
    public ConsoleColor DebugColor { get; set; }
    public ConsoleColor InformationColor { get; set; }
    public ConsoleColor NoneColor { get; set; }
}